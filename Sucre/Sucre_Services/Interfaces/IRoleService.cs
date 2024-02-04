using Sucre_Core;
using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IRoleService
    {        

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="appRoleDto"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(AppRoleDto appRoleDto);            
        //Task<bool> UpdateRole(AppRoleTdo appRoleTdo);
        /// <summary>
        /// Get a role by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppRoleDto> GetAppRoleAsync(Guid id);
        /// <summary>
        /// Get list of roles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppRoleDto>> GetListRolesAsync();
        Task<IEnumerable<AppRoleDto>> GetListRolesByUserIdAsync(Guid id);
        /// <summary>
        /// Get a app role and assigned app user. Get app user not 
        /// assigned role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUsersAssigned"></param>
        /// <param name="appUsersNotAssigned"></param>
        /// <returns></returns>
        AppRoleDto GetAppRoleUsers(Guid id,
            ref IEnumerable<AppUserDto> appUsersAssigned,
            ref IEnumerable<AppUserDto> appUsersNotAssigned);
        /// <summary>
        /// Removing/adding a user associated with a role
        /// </summary>
        /// <param name="idRole">id role</param>
        /// <param name="idUser">id user</param>
        /// <param name="action">action performed on the role</param>
        /// <returns></returns>
        Task<bool> ListAppRoleUsersActionAsync(Guid idRole, Guid idUser, ActionRoleUser action);        
        /// <summary>
        /// Create or update a role
        /// </summary>
        /// <param name="appRoleTdo"></param>        
        /// <returns></returns>
        Task<bool> UpsertRoleAsync(AppRoleDto appRoleTdo);
    }
}
