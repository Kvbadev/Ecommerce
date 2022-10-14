using AutoMapper;
using Core;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;
using Microsoft.EntityFrameworkCore;
using Web.Services;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _jwtTokenService;
    public AccountController(DataContext context, UserManager<AppUser> userManager, IMapper mapper,
            ILogger<AccountController> logger, IConfiguration configuration, SignInManager<AppUser> signInManager,
            IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _userManager = userManager;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto user,
        [FromServices]ILogger logger)
    {

        var newUser = new AppUser();
        _mapper.Map(user, newUser); 

        var result = await _userManager.CreateAsync(newUser, user.Password);

        if(result.Succeeded)
        {
            newUser.ShoppingCart = new ShoppingCart();
            logger.LogInformation("New user {} has been created", user.Username);

            var accessToken = _jwtTokenService.GenerateAccessToken(newUser);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            newUser.RefreshToken = refreshToken;
            newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var res = await _context.SaveChangesAsync() > 0;

            if(res)
            {
                return Ok(new AuthResponse{
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            return BadRequest("Could not register a new user");
        }
        return BadRequest(result.Errors.ElementAt(0).Description.ToString()); //return one of the errors
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto creds,
        [FromServices]SignInManager<AppUser> manager)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == creds.Username);
        if(user == null){
            return Unauthorized("This username does not exist");
        }
        
        var res = await manager.CheckPasswordSignInAsync(user, creds.Password, false);

        if(res.Succeeded){
            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var outcome = await _context.SaveChangesAsync() > 0;

            return outcome ?
            Ok(new AuthResponse{
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }) :
            BadRequest("Could not persist changes");
        }
        return BadRequest("Invalid Password");
    }

    [HttpGet("profile")]
    public async Task<Core.Profile?> Profile() 
    {
        var user = await _context.Users.FindAsync(_jwtTokenService.ExtractId());
        if(user is null)
        {
            return null;
        }

        return _mapper.Map<Core.AppUser, Core.Profile>(user);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> UpdateProfile(Core.Profile updatedProf)
    {
        var user = await _context.Users.FindAsync(_jwtTokenService.ExtractId());
        if(user is null)
        {
            return BadRequest("User does not exist");
        }
        
        var res=await _userManager.UpdateAsync(_mapper.Map<Core.Profile, AppUser>
                                      (updatedProf, user));
        return res.Succeeded ? Ok() :
        BadRequest(res.Errors);
    }

    [HttpGet("transactions")]
    public async Task<IEnumerable<TransactionDto>> Transactions()
    {
        var user = await _context.Users.Include(x => x.Transactions)
        .ThenInclude(x => x.Products).ThenInclude(x => x.Product)
        .FirstAsync(x => x.Id == _jwtTokenService.ExtractId());

        if(user == null || user.Transactions.Count <= 0)
        {
            return Enumerable.Empty<TransactionDto>();
        }
        var transactions = _mapper.Map<Transaction[], IEnumerable<TransactionDto>>
                            (user.Transactions.ToArray()).Where(x => x.Success);
        
        return transactions.Any() ? transactions : 
        Enumerable.Empty<TransactionDto>();
    }
}