using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class PointM
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
        public int CexId { get; set; }
        [MaxLength(20)]
        public string? ServiceStaff { get; set; } = string.Empty;
    }
}
