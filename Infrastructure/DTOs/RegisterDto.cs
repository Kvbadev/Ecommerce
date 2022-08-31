using System.ComponentModel.DataAnnotations;

namespace Core;

public class RegisterDto
{
    [Required]
    public string? Firstname { get; set; }
    
    [Required]
    public string? Lastname { get; set; }

    [Required, MinLength(4, ErrorMessage = "Your username should be more than 4 characters"), MaxLength(16, ErrorMessage = "Your username should be less than 16 characters")]
    public string? Username { get; set; }
    
    [Required, RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "This is not a correct email address")]
    public string? Email { get; set; }

    // [Required, RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Your password should contain more than 7 characters, one letter and one number")]
    [Required]
    public string? Password { get; set; }
}
