using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class AppRoleM
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(12)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Value { get; set; }

        public bool? IsEdit { get; set; }
    }
}
