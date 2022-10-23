using Core;
using Microsoft.AspNetCore.Identity;

namespace Web.Services;
public interface IJwtTokenService
{
    Task<string> GenerateAccessToken(AppUser user);
    string GenerateRefreshToken();
    public string ExtractId();
    public string ExtractId(string token);
}