using Core;

namespace Web.Services;
public interface IJwtTokenService
{
    string GenerateAccessToken(AppUser user);
    string GenerateRefreshToken();
    public string ExtractId();
    public string ExtractId(string token);
}