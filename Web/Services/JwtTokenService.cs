using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core;
using Google.Apis.Auth;
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
            Audience = "https://jakubcommerce.azurewebsites.net/",
            Issuer = "https://jakubcommerce.azurewebsites.net/",
            Subject = new ClaimsIdentity(MyClaims),
            Expires = DateTime.UtcNow.AddMinutes(10),
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
        try
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
        catch(Exception) //usually when unable to read corrupted token
        {
            return string.Empty;
        }
    }
    public string ExtractId(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var can = handler.CanReadToken(token);
            if(token == string.Empty || !handler.CanReadToken(token))
            {
                return string.Empty;
            }
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var nameId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; //unfortunately claimtypes.nameidentifier does not seem to work
            return nameId ?? string.Empty;
        }
        catch(Exception) //usually when unable to read corrupted token
        {
            return string.Empty;
        }

    }
    public async Task<GoogleJsonWebSignature.Payload?> VerifyToken(string token)
    {
        try{
            //throw on invalid token
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            return payload;
        }
        catch(Exception)
        {
            return null;
        }
    }
    
    public string GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return "";
    }

}