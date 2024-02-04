using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    public class DeviceTagsM
    {   
        public int Id { get; set; }
        public string NameDevice { get; set; }
        public List<TagM> TagsM {  get; set; } 
        public int AddChannale { get; set; }
        public List<SelectListItem> FreeTagsM { get; set; }

    }
}
