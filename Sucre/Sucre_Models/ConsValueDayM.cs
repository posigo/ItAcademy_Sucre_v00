using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class ConsValueDayM
    {
        public int PointId { get; set; } = 0;
        public List<SelectListItem>? PointNameSelectList { get; set; }
        [Range(1, 31, ErrorMessage = "Not correct day")]
        public int NDay { get; set; } = DateTime.Now.AddDays(-1).Day;
        [Range(1, 12, ErrorMessage = "Not correct month")]
        public int NMonth { get; set; } = DateTime.Now.AddDays(-1).Month;
        [Range(minimum:2023 , maximum: 2034, ErrorMessage = "Not correct year")]
        public int NYear { get; set; } = DateTime.Now.AddDays(-1).Year;
        [Range(1, 31, ErrorMessage = "Not correct day")]
        public int? NDayE { get; set; }
        [Range(1, 12, ErrorMessage = "Not correct month")]
        public int? NMonthE { get; set; }
        [Range(minimum: 2023, maximum: 2034, ErrorMessage = "Not correct year")]
        public int? NYearE { get; set; }
        
    }
    
}
