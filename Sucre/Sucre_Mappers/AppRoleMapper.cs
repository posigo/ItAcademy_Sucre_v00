using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class AppRoleMapper
    {
        public partial AppRoleDto AppRoleToAppRoleDto(AppRole appRole);
        public partial AppRole AppRoleDtoToAppRole(AppRoleDto appRoleDto);        
    }
}
