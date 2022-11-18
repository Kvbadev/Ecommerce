using System.ComponentModel.DataAnnotations;

namespace Core;

public class RegisterDto
{
    public string Firstname { get; set; } = default!;
    
    public string Lastname { get; set; } = default!;

    public string Username { get; set; } = default!;
    
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}
