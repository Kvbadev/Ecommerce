using AutoMapper;
using Core;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;
using Microsoft.EntityFrameworkCore;
using Web.Services.JwtToken;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountController>  _logger;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    public AccountController(DataContext context, UserManager<AppUser> userManager, IMapper mapper,
            ILogger<AccountController> logger, IConfiguration configuration, SignInManager<AppUser> signInManager,
            IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto user)
    {

        var newUser = new AppUser();
        _mapper.Map(user, newUser); 

        var result = await _userManager.CreateAsync(newUser, user.Password);

        if(result.Succeeded)
        {
            newUser.ShoppingCart = new ShoppingCart();
            _logger.LogInformation("New user {} has been created", user.Username);

            var token = _jwtTokenService.GenerateToken(newUser);

            var res = await _context.SaveChangesAsync() > 0;
            if(res)
            {
                return Ok("Shopping cart has been cleared");
            }
            return BadRequest("Could not clear the cart");
        }
        return BadRequest(result.Errors.ElementAt(0).Description.ToString()); //return one of the errors
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto creds)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == creds.Username);
        if(user == null){
            return Unauthorized("This username does not exist");
        }
        
        var res = await _signInManager.CheckPasswordSignInAsync(user, creds.Password, false);
        if(res.Succeeded){
            var token = _jwtTokenService.GenerateToken(user);
            return Ok(token);
        }
        return BadRequest("Invalid Password");
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<Core.Profile?> Profile() 
    {
        
        var userId = _jwtTokenService.ExtractId();
        if(userId == string.Empty)
        {
            return null;
        }

        Core.Profile? userProfile = new Core.Profile();
        AppUser? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if(user == null){
            return null;
        } 

        _mapper.Map<Core.AppUser, Core.Profile>(user, userProfile);
        return userProfile; 
    }
}