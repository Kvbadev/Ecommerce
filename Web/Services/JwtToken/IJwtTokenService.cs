using Core;

namespace Web.Services.JwtToken;
public interface IJwtTokenService
{
    string GenerateToken(AppUser user);
    public string ExtractId();
}