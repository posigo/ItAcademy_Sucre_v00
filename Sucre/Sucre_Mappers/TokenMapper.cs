using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class TokenMapper
    {
        public partial RefreshTokenDto RefreshTokenToRefreshTokenDto(RefreshToken refreshToken);
        public partial RefreshToken RefreshTokenDtoToRefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
