using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    public class PointUpsertM
    {
        public PointM PointM { get; set; }
        public IEnumerable<SelectListItem> EnergySelectList { get; set; }
        public IEnumerable<SelectListItem> CexSelectList { get; set; }


    }
}
