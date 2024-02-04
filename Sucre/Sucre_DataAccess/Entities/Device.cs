using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    public class Device: IBaseEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Connection { get; set; }
    }
}
