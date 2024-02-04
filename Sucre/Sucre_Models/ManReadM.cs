using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class ManReadM
    {
        [Required]
        
        public int IdChannale { get; set; }
        [Required]
        [DataType(DataType.Duration)]
        [Range(2023,2033,ErrorMessage ="Error year")]
        public int YearB { get; set; }
        [Required]
        [Range(1,12, ErrorMessage = "Error month")]
        public int MonthB { get; set; }
        [Required]
        [Range (1,31, ErrorMessage = "Error day")]         
        public int DayB { get; set; }
        [Range(0,23)]
        public int? Hour { get; set; }
        
        [Range(2023, 2033)]
        public int? YearE { get; set; }
        [Range(1, 12)]
        public int? MonthE { get; set; }
        [Range(1, 31)]
        public int? DayE { get; set; }
        public string Result { get; set; } = "";
    }
}
