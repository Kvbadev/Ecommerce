using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController: ControllerBase
{
    private readonly DataContext _context;
    private readonly IJwtTokenService _jwt;
    public TokenController(DataContext context, IJwtTokenService jwt)
    {
        _jwt = jwt;
        _context = context;
    }

    [Route("refresh"), HttpGet]
    public async Task<IActionResult> Refresh(AuthResponse tokens)
    {
        if(tokens.AccessToken == string.Empty || tokens.RefreshToken == string.Empty)
        {
            return BadRequest("Invalid tokens");
        }
        var user = await _context.Users.
                         FindAsync(_jwt.ExtractId(tokens.AccessToken!));

        if(user is null || user.RefreshToken != tokens.RefreshToken ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            return BadRequest("Something went wrong");
        }

        var refresh = _jwt.GenerateRefreshToken();
        var access = _jwt.GenerateAccessToken(user);

        user.RefreshToken = refresh;

        var res = await _context.SaveChangesAsync() > 0;

        return res ?
        Ok(new AuthResponse{
            RefreshToken = refresh,
            AccessToken = access
        }) : 
        BadRequest("Could not persist changes");
    }
}

