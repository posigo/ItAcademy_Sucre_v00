using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Role
    /// </summary>
    [Table("AppRoles")]
    public class AppRole:IBaseEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(12)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Value { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }

        public AppRole()
        {
            this.AppUsers = new HashSet<AppUser>();
        }


    }
}
