using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class AppUserLoginM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //[Required]
        public string? ReturnUrl { get; set; }

    }
}
