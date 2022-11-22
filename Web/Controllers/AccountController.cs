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
using AutoMapper.QueryableExtensions;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Controllers;

public class AccountController : DefaultController
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

        var updatedUser = _mapper.Map<RegisterDto, AppUser>(user); 

        updatedUser.RefreshToken = _jwtTokenService.GenerateRefreshToken();
        updatedUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        var message = await ValidateEntity(updatedUser);
        if(message != string.Empty)
        {
            return BadRequest(message);
        }

        var result = await _userManager.CreateAsync(updatedUser, user.Password);
        await _userManager.AddToRoleAsync(updatedUser, "User");


        if(result.Succeeded)
        {
            _logger.LogInformation("New user {} has been created", user.Username);
            var accessToken = await _jwtTokenService.GenerateAccessToken(updatedUser);

            return Ok(new AuthResponse{
                AccessToken = accessToken,
                RefreshToken = updatedUser.RefreshToken
            });
        }
        return BadRequest(result.Errors.ElementAt(0)?.Description ??
        "Could not register a new user");
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

    [AllowAnonymous]
    [HttpPost("GoogleSign")]
    public async Task<IActionResult> GoogleLogin([FromBody]string IdToken)
    {
        var credentials = await _jwtTokenService.VerifyToken(IdToken); //null on invalid
        if(credentials == null)
        {
            return BadRequest("Invalid token");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == credentials.Email);

        if(user == null)
        {
            var newUser = new AppUser {
                UserName = credentials.GivenName+new Random().Next(99999),
                Firstname = credentials.GivenName ?? "Firstname",
                Lastname = credentials.FamilyName ?? "Lastname",
                Email = credentials.Email
            };

            var message = await ValidateEntity(newUser);
            if(message != string.Empty)
            {
                return BadRequest(message);
            }

            newUser.RefreshToken = _jwtTokenService.GenerateRefreshToken();
            newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var outcome = await _userManager.CreateAsync(newUser);
            await _userManager.AddToRoleAsync(newUser, "User");

            return outcome.Succeeded ? 
            Ok(new AuthResponse{
                AccessToken = await _jwtTokenService.GenerateAccessToken(newUser),
                RefreshToken = newUser.RefreshToken
            }) : 
            BadRequest("Could not create a user");
        }
        
        user.RefreshToken = _jwtTokenService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        var res = await _context.SaveChangesAsync() > 0;

        return res ? Ok(new AuthResponse {
            AccessToken = await _jwtTokenService.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        }) : 
        BadRequest("Could not log user in");
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
        var updatedUser = _mapper.Map<Core.Profile, AppUser>(updatedProf);

        var message = await ValidateEntity<AppUser>(updatedUser);
        if(message != string.Empty)
        {
            return BadRequest(message);
        }
        
        var res = await _userManager.UpdateAsync(updatedUser);

        return res.Succeeded ? Ok() :
        BadRequest(res.Errors);
    }

    [HttpGet("transactions")]
    public async Task<IEnumerable<TransactionDto>> Transactions()
    {
        var id = _jwtTokenService.ExtractId();

        var user = await _context.Users.Include(x => x.Transactions)
        .ThenInclude(x => x.Products).ThenInclude(x => x.Product.Photos)
        .FirstAsync(x => x.Id == _jwtTokenService.ExtractId());

        if(user == null || user.Transactions.Count <= 0)
        {
            return Enumerable.Empty<TransactionDto>();
        }
        // var transactions = _mapper.Map<Transaction[], IEnumerable<TransactionDto>>
        //                     (user.Transactions.ToArray()).Where(x => x.Success);
        // var transactions = user.Transactions.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider);
        var transactions = _mapper
            .Map<ICollection<Transaction>, IEnumerable<TransactionDto>>(user.Transactions);

        return transactions.Any() ? transactions : 
        Enumerable.Empty<TransactionDto>();
    }

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

        foreach(var c in clients)
        {
            c.Privileges = await _userManager.GetRolesAsync
                (_context.Users.FirstOrDefault(x => x.UserName == c.Username)!)
                ?? Enumerable.Empty<string>();
        }

        return clients;
    }

}