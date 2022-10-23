using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
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

    [Route("refresh"), HttpPatch]
    public async Task<IActionResult> Refresh([FromBody]AuthResponse req)
    {
        var user = await _context.Users.
                         FindAsync(_jwt.ExtractId(req.AccessToken ?? ""));

        if(user is null)
        {
            return BadRequest("Invalid user");
        } 
        if(user.RefreshToken != req.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            return BadRequest("Invalid refresh token");
        }

        var refresh = _jwt.GenerateRefreshToken();
        var access = await _jwt.GenerateAccessToken(user);

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

