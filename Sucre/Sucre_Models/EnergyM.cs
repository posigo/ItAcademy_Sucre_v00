using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class EnergyM
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(15)]
        [Display(Name ="Тип энергосреды")]
        public string EnergyName { get; set; } = string.Empty;
    }
}
