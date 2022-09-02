using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core;
using Microsoft.IdentityModel.Tokens;

namespace Web.Services.JwtTokenService;
public class JwtTokenService
{
    public static string GenerateToken(AppUser user, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["JwtKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = "http://localhost:5000",
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
    public static string? ExtractId(string authHeader)
    {
        string token = authHeader.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var nameId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value; //unfortunately claimtypes.nameidentifier does not seem to work
        return nameId;
    }
}