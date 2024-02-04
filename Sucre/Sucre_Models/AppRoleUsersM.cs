using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    /// <summary>
    /// список каналов в точке учёта
    /// </summary>
    public class AppRoleUsersM
    {
        /// <summary>
        /// Id роли
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя роли
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// колеккция users привязанных к роли
        /// </summary>
        public ICollection< AppUserM> AppUsersM { get; set; } = new HashSet<AppUserM>();
        /// <summary>
        /// Список user не привязанных к роли 
        /// </summary>
        public IEnumerable<SelectListItem> FreeAppUsersSelectList { get; set; }

        public Guid AddUser { get; set; } = Guid.Empty;
                
    }
}
