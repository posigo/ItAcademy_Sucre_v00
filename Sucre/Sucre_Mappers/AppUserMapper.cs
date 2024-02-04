using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class AppUserMapper
    {
        public partial AppUserDto AppUserToAppUserDto(AppUser appUser);
        public partial AppUser AppUserDtoToAppUser(AppUserDto appUserDto);        
    }
}
