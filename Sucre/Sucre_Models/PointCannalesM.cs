using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    /// <summary>
    /// список каналов в точке учёта
    /// </summary>
    public class PointCannalesM
    {
        /// <summary>
        /// Id точки учёта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Имя точки учёта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// колеккция каналов привязанных к точке учёта
        /// </summary>
        public IEnumerable<CanalM> CannalesM { get; set; } = new HashSet<CanalM>();
        /// <summary>
        /// Список каналов не привязанных к точек учёта
        /// </summary>
        public IEnumerable<SelectListItem> FreeCanalesSelectList { get; set; }

        public int AddCannale { get; set; } = 0;
    }
}
