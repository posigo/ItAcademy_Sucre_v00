using Sucre_Core;
using Sucre_Core.DTOs;
using System.Security.Claims;

namespace Sucre_Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// User authentication by email
        /// </summary>
        /// <param name="email">email user</param>
        /// <param name="super">Super user</param>
        /// <returns></returns>
        public Task<ClaimsIdentity> Authenticate(string useName, bool super = false);
        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserIdAsync(Guid Id);
        /// <summary>
        /// Get a user with email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AppUserDto> GetAppUserAsync(string email);        
        /// <summary>
        /// Get a user with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUserDto> GetAppUserAsync(Guid id);
        /// <summary>
        /// Get app user by refresh token Id
        /// </summary>
        /// <param name="refreshToken">Id refresh token</param>
        /// <returns></returns>
        Task<AppUserDto> GetAppUserByRefreshTokenAsync(Guid refreshToken);
        /// <summary>
        /// Get a app user and assigned app role. Get app role not 
        /// assigned role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUsersAssigned"></param>
        /// <param name="appUsersNotAssigned"></param>
        /// <returns></returns>
        AppUserDto GetAppUserRoles(
            Guid id,
            ref IEnumerable<AppRoleDto> appRolesAssigned,
            ref IEnumerable<AppRoleDto> appRolesNotAssigned);
        /// <summary>
        /// Get list users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppUserDto>> GetListUsersAsync();
        /// <summary>
        /// Full user name as a string
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string GetStringAppUserAsync(Guid Id);
        /// <summary>
        /// Password verification
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> IsPasswordCorrect(string password, string email = null);
        /// <summary>
        /// User verification
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> IsUserExist(string email);
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="userTdo"></param>
        /// <returns></returns>
        public Task<int> RegisterUserAsync(AppUserRegTdo userTdo);

        string getmd5hash(string sss);

        /// <summary>
        /// Removing/adding a role associated with a user
        /// </summary>
        /// <param name="idUser">id user</param>
        /// <param name="idRole">id role</param>
        /// <param name="action">action performed on the user</param>
        /// <returns></returns>
        Task<bool> ListAppUserRolesActionAsync(Guid idUser, Guid idRole, ActionRoleUser action = ActionRoleUser.None);
        /// <summary>
        /// Creating or updating user
        /// </summary>
        /// <param name="appUserDto"></param>
        /// <returns></returns>
        Task<bool> UpsertUserAsync(AppUserDto appUserDto);

    }
}
