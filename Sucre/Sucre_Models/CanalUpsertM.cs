using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    public class CanalUpsertM
    {
        public CanalM CanalM { get; set; }
        public IEnumerable<SelectListItem> ParametryTypeSelectList { get; set; }
        public CanalUpsertM()
        {
                this.CanalM = new CanalM();
        }
    }
}
