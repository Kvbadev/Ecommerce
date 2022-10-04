using Core;

namespace Web.Services;
public interface IJwtTokenService
{
    string GenerateToken(AppUser user);
    public string ExtractId();
}