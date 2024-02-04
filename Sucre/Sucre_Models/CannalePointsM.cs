using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    /// <summary>
    /// список точек учёта привязанных к каналу
    /// </summary>
    public class CannalePointsM
    {
        /// <summary>
        /// Id канала
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя канала
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// колеккция точек учёта привязанных к каналу
        /// </summary>
        public IEnumerable<PointM> PointsM { get; set; } = new HashSet<PointM>();
        /// <summary>
        /// Список точек учёта не привязанных к каналу
        /// </summary>
        public IEnumerable<SelectListItem> FreePointsSelectList { get; set; }

        public int AddPoint { get; set; } = 0;

        public CannalePointsM()
        {
            //PointM = new PointM();
            //CannalesM = HashSet<CanalM>();
        }
    }
}
