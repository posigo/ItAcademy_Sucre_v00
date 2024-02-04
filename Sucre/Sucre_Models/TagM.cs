using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    public class TagM
    {
        public int Id { get; set; }
        public int ChannaleId { get; set; }
        public IEnumerable<SelectListItem> ChannalesTypeSelectList { get; set; }
        public int DeviceId { get; set; }
        public  string DeviceName { get; set; } = string.Empty;
        public int Enviroment { get; set; }
        public IEnumerable<SelectListItem> EnviromentsTypeSelectList { get; set; }
        public int ParameterCode{ get; set; }
        public IEnumerable<SelectListItem> ParametersTypeSelectList { get; set; }


    }
}
