using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class AppUserM
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; }
        [Required]
        public int GroupNumber { get; set; }
        //public IEnumerable<SelectListItem> GroupSelectList { get; set; }

        public bool IsEdit { get; set; }

    }
}
