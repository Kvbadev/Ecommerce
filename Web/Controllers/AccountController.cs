using AutoMapper;
using Core;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Services.JwtTokenService;
using Web.DTOs;
using Microsoft.EntityFrameworkCore;

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
    public AccountController(DataContext context, UserManager<AppUser> userManager, IMapper mapper,
            ILogger<AccountController> logger, IConfiguration configuration, SignInManager<AppUser> signInManager)
    {
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
        // var doesExist = _context.Users.Any(x => x.UserName == user.Username);
        // if(doesExist)
        // {
        //     return BadRequest("This username is already registered");
        // }

        var newUser = new AppUser();

        _mapper.Map(user, newUser); 

        var result = await _userManager.CreateAsync(newUser, user.Password);

        if(result.Succeeded)
        {
            _logger.LogInformation("New user {} has been created", user.Username);
            var token = JwtTokenService.GenerateToken(newUser, _configuration);

            return Ok(token.ToString());
        }
        return BadRequest(result.Errors.ElementAt(0).Description.ToString());
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
            var token = JwtTokenService.GenerateToken(user, _configuration);
            return Ok(token);
        }
        return BadRequest("Invalid Password");
    }

    [HttpGet("profile")]
    public async Task<Core.Profile?> Profile() 
    {
        var username = HttpContext.User.Identity?.Name;
        if(username == null)
        {
            return null;
        }
        Core.Profile userProfile = new Core.Profile();
        AppUser? user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if(user == null){
            return null;
        } 
        _mapper.Map<Core.AppUser, Core.Profile>(user, userProfile);

        return userProfile; 
        // string authHeader = HttpContext.Request.Headers["Authorization"];
        // if(authHeader == string.Empty || authHeader.StartsWith("Bearer"))
        // {
        //     return null;
        // }
        // var id = JwtTokenService.ExtractId();
    }
}