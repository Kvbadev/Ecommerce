using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.IdentityModel.Tokens;
using Web.ExtensionMethods;

namespace Web.Services.JwtToken;
public class JwtTokenService : IJwtTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken(AppUser user)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = "https://localhost:5000",
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public string ExtractId()
    {
        string token = _httpContextAccessor.GetBearerToken();
        if(token == string.Empty)
        {
            return string.Empty;
        }
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var nameId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; //unfortunately claimtypes.nameidentifier does not seem to work
        return nameId ?? string.Empty;
    }
}