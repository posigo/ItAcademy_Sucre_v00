using System.ComponentModel.DataAnnotations;

namespace Sucre_WebApi.Models
{
    /// <summary>
    /// Точки учета
    /// </summary>
    public class PointCreateApiM
    {
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

    }
}
