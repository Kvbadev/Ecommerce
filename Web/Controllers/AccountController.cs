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
[Authorize(Roles = "User")]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger _logger;
    public AccountController(DataContext context, UserManager<AppUser> userManager, IMapper mapper,
           IConfiguration configuration, SignInManager<AppUser> signInManager,
           ILogger<AccountController> logger, IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto user)
    {

        var newUser = new AppUser();
        _mapper.Map(user, newUser); 

        newUser.CreationDate = DateTime.UtcNow;
        newUser.RefreshToken = _jwtTokenService.GenerateRefreshToken();
        newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        var result = await _userManager.CreateAsync(newUser, user.Password);
        await _userManager.AddToRoleAsync(newUser, "User");


        if(result.Succeeded)
        {
            newUser.ShoppingCart = new ShoppingCart();
            // _logger.LogInformation("New user {} has been created", user.Username);
            var accessToken = await _jwtTokenService.GenerateAccessToken(newUser);

            var res = await _context.SaveChangesAsync() > 0;

            if(res)
            {
                return Ok(new AuthResponse{
                    AccessToken = accessToken,
                    RefreshToken = newUser.RefreshToken
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
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == creds.Username);
        if(user == null){
            return Unauthorized("This username does not exist");
        }
        
        var res = await manager.CheckPasswordSignInAsync(user, creds.Password, false);

        if(res.Succeeded){
            var accessToken = await _jwtTokenService.GenerateAccessToken(user);
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
        var id = _jwtTokenService.ExtractId();
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

    [Authorize]
    [HttpGet("isAdmin")]
    public async Task<bool> isAdmin()
    {
        var user = await _context.Users.FindAsync(_jwtTokenService.ExtractId());
        if(user is null)
        {
            return false;
        }
        return await _userManager.IsInRoleAsync(user, "Administrator") ?
        true :
        false;
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("clients")]
    public async Task<IEnumerable<ClientDto>> GetClients()
    {
        var users = await _context.Users.Include(x => x.Transactions).ToArrayAsync();
        if(users.Length is 0)
        {
            return Enumerable.Empty<ClientDto>();
        }

        var clients = _mapper.Map<AppUser[], IEnumerable<ClientDto>>(users);

        var test = _context.Users.FirstOrDefault(x => x.UserName == "jakub");
        var trans = test!.Transactions;
        var data = trans.Aggregate(0M,(a,b) => a + b.Price);

        foreach(var c in clients)
        {
            c.Privileges = await _userManager.GetRolesAsync
                (_context.Users.FirstOrDefault(x => x.UserName == c.Username)!)
                ?? Enumerable.Empty<string>();
        }

        return clients;
    }

}