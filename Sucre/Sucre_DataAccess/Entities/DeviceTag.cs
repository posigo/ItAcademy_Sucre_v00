using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    public class DeviceTag:IBaseEntity<int>
    {
        public int Id { get; set; }
        public int ChannaleId { get; set; }
        public int DeviceId { get; set; }
        public int Enviroment {  get; set; }
        public int ParameterCode {get; set; }
    }
}
