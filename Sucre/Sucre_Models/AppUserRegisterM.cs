using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class AppUserRegisterM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
        //[Required]
        public string? ReturnUrl { get; set; }

    }
}
