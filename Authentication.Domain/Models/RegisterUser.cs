using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Models;

public class RegisterUser
{
    [Required(ErrorMessage = "User Name is Requireed")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is Requireed")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is Requireed")]
    public string? Password { get; set; }
}
