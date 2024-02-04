using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    [Table("Reports")]
    public class Report
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<GroupUser> GroupUsers { get; set; }
        //public virtual ICollection<ReportDetail> ReportDetails { get; set; }

        public Report()
        {
            this.GroupUsers = new HashSet<GroupUser>();
            //this.ReportDetails = new HashSet<ReportDetail>();
        }
    }
}
