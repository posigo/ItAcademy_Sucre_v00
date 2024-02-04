using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Точки учета
    /// </summary>
    [Table("Points")]
    public class Point : IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int EnergyId { get; set; }
        [Required]
        public int CexId { get; set;}
        [MaxLength(20)]
        public string? ServiceStaff { get; set; } = string.Empty; 

        [ForeignKey("EnergyId")]
        public virtual Energy Energy { get; set; }
        [ForeignKey("CexId")]
        public virtual Cex Cex { get; set;}
        public virtual ICollection<Canal> Canals { get; set; }
        //public virtual ICollection<ReportDetail> ReportDetails { get; set; }

        public Point()
        {
            this.Canals = new HashSet<Canal>();
            //this.ReportDetails = new HashSet<ReportDetail>();
        }
    }
}
