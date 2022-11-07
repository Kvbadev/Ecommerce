using Core;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[ApiController]
[Authorize(Roles = "Administrator")]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;
    public RolesController(DataContext context, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpPatch("update/{username}")]
    public async Task<IActionResult> UpdateUserRoles([FromRoute]string username,
                                     [FromBody]IEnumerable<string> roles)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if(user is null)
        {
            return BadRequest("Invalid username");
        }

        var old_roles = await _userManager.GetRolesAsync(user);
        var res = await _userManager.AddToRolesAsync(user, roles.Except(old_roles));

        if(res != IdentityResult.Success)
        {

            return BadRequest("Could not add user to roles");
        } 

        var to_remove = old_roles.Except(roles);
        if(await _userManager.RemoveFromRolesAsync(user, to_remove) != IdentityResult.Success)
        {
            return BadRequest("Could not remove user from roles");
        }

        return Ok();

    }
}