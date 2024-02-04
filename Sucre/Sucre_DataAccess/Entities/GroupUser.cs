using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Группа user
    /// </summary>
    [Table("GroupUsers")]
    public class GroupUser:IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]        
        public int Number { get; set; } = 0;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        //public virtual ICollection<AppUser>? AppUsers { get; set;}
        public virtual ICollection<Report> Reports { get; set;}
        
        public GroupUser()
        {
            //this.AppUsers = new HashSet<AppUser>();
            this.Reports = new HashSet<Report>();
        }

    }
}