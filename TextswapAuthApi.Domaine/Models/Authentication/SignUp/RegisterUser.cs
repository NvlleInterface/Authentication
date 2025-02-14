using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextswapAuthApi.Models.Models.Authentication.SignUp
{
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
}
