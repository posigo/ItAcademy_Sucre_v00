using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// User
    /// </summary>
    [Table("AppUsers")]
    public class AppUser:IBaseEntity<Guid>
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
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public int GroupNumber { get; set; } = 99;               

        //[ForeignKey("GroupId")]
        //public virtual GroupUser GroupUser { get; set; }
        public virtual ICollection<AppRole> AppRoles { get; set; }

        public AppUser()
        {
            this.AppRoles = new HashSet<AppRole>();
        }
    }
}
