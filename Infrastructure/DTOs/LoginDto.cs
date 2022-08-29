using System.ComponentModel.DataAnnotations;

namespace Web.DTOs;

public class LoginDto
{
    [Required, MinLength(4, ErrorMessage = "Your username should be more than 4 characters"), MaxLength(16, ErrorMessage = "Your username should be less than 16 characters")]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
