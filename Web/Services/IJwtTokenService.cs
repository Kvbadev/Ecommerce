using Core;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace Web.Services;
public interface IJwtTokenService
{
    public Task<string> GenerateAccessToken(AppUser user);
    string GenerateRefreshToken();
    public string ExtractId();
    public string ExtractId(string token);
    public string GetClaims(string token);
    public Task<GoogleJsonWebSignature.Payload?> VerifyToken(string token);
}