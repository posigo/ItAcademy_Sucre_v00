using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_Models
{
    /// <summary>
    /// список ролей в юзере
    /// </summary>
    public class AppUserRolesM
    {
        /// <summary>
        /// Id user
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// колеккция roles привязанных к user
        /// </summary>
        public ICollection< AppRoleM> AppRolesM { get; set; } = new HashSet<AppRoleM>();
        /// <summary>
        /// Список roles не привязанных к user 
        /// </summary>
        public IEnumerable<SelectListItem> FreeAppRolesSelectList { get; set; }

        public Guid AddRole { get; set; } = Guid.Empty;
                
    }
}
