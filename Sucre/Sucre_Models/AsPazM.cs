using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    /// <summary>
    /// АС и ПАЗ
    /// </summary>
    public class AsPazM
    {
        [Key]
        public int Id { get; set; }
        public decimal? High { get; set; } = null;
        public decimal? Low { get; set; } = null;
        public decimal? A_High { get; set; } = null;
        public decimal? W_High { get; set; } = null;
        public decimal? W_Low { get; set; } = null;
        public decimal? A_Low { get; set; } = null;
        public bool A_HighEin { get; set; } = false;
        public bool W_HighEin { get; set; } = false;
        public bool W_LowEin { get; set; } = false;
        public bool A_LowEin { get; set; } = false;
        public bool A_HighType { get; set; } = false;
        public bool W_HighType { get; set; } = false;
        public bool W_LowType { get; set; } = false;
        public bool A_LowType { get; set; } = false;        
        
        public int CanalId { get; set; }
    }
}
