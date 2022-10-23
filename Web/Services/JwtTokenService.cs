using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Web.ExtensionMethods;

namespace Web.Services;
public class JwtTokenService : IJwtTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    public JwtTokenService(IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GenerateAccessToken(AppUser user)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);

        var MyClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        //roles
        foreach(var role in await _userManager.GetRolesAsync(user))
        {
            MyClaims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //cannot remove audience here even though it is alredy set in program.cs because then the system breaks
            Audience = "https://localhost:5000",
            Issuer = "https://localhost:5000",
            Subject = new ClaimsIdentity(MyClaims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using(var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }

    public string ExtractId()
    {
        string token = _httpContextAccessor.GetBearerToken();
        var handler = new JwtSecurityTokenHandler();
        if(token == string.Empty || !handler.CanReadToken(token))
        {
            return string.Empty;
        }
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var nameId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; //unfortunately claimtypes.nameidentifier does not seem to work
        return nameId ?? string.Empty;
    }
    public string ExtractId(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if(token == string.Empty || !handler.CanReadToken(token))
        {
            return string.Empty;
        }
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var nameId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; //unfortunately claimtypes.nameidentifier does not seem to work
        return nameId ?? string.Empty;
    }

}