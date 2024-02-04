using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    public class ConsValueHourM
    {
        public int PointId { get; set; } = 0;
        public List<SelectListItem>? PointNameSelectList { get; set; }
        [Range(1, 32, ErrorMessage = "Not correct day")]
        public int NDay { get; set; } = DateTime.Now.Day;
        [Range(1, 13, ErrorMessage = "Not correct month")]
        public int NMonth { get; set; } = DateTime.Now.Month;
        [Range(minimum:2023 , maximum: 2034, ErrorMessage = "Not correct year")]
        public int NYear { get; set; } = DateTime.Now.Year;
        [Range(0, 24, ErrorMessage = "Not correct hour")]
        public int NHour { get; set;} = DateTime.Now.Hour;
    }
    
}
