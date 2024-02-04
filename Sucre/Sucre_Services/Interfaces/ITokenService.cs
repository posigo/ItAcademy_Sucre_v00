using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(AppUserDto userDto, List<AppRoleDto> appRolesDto);
        Task<List<RefreshTokenDto>> GetRefreshTokens();
        Task<RefreshTokenDto> GetRefreshTokenById(Guid id);
        Task<List<RefreshTokenDto>> GetRefreshTokensByUser(Guid id);
        Task<Guid> AddRefreshToken(string reqestEmail, string ipAddress);
        Task<bool> CheckRefreshToken(Guid requestRefreshToken);
        Task RemoveRefreshToken(Guid requestRefreshToken);
    }
}