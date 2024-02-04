using LinqKit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sucre_Core;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Sucre_Services
{
    public class UserService : IUserService
    {
        private readonly AppUserMapper _appUserMapper;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;

        public UserService(
            AppUserMapper appUserMapper,
            IConfiguration configuration, 
            IMediator mediator,
            ISucreUnitOfWork sucreUnitOfWork)
        {
            _appUserMapper = appUserMapper;
            _configuration = configuration;
            _mediator = mediator;
            _sucreUnitOfWork = sucreUnitOfWork;
        }

        /// <summary>
        /// User authentication by email
        /// </summary>
        /// <param name="email">email user</param>
        /// <param name="super">Super user</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Authenticate(string email, bool super = false)
        {
            try
            {

                string role = super ? WC.SupervisorRole : GetStringTypeRole(email).Result;
                //var groupId = super ? "999" : GetStringGropId(email).Result;
                var groupId = super ? "999" : "99";

                var claims = new List<Claim>()
                {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                //new Claim(ClaimTypes.Role,role.ToString()),
                new Claim("GroupId",groupId.ToString())
                };
                if (!role.IsNullOrEmpty())
                    claims.Add(new Claim(ClaimTypes.Role, role));

                var claimsIdentity = new ClaimsIdentity(claims,
                    "ApplicationCookie",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"public async Task<ClaimsIdentity> Authenticate(string {email}");
            }
            return null;
        }

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserIdAsync(Guid Id)
        {
            try
            {
                await _sucreUnitOfWork.repoSucreAppUser.RemoveByIdAsync(Id);
                _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error delete user");
                return false;
            }

        }

        /// <summary>
        /// generating code-hash using the MD5 algorithm
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GenerateMD5Hash(string str)
        {
            var salt = _configuration["AppSettings:PasswordSalt"];
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes($"{str}{salt}");
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var res = Convert.ToHexString(hashBytes);

                return Convert.ToHexString(hashBytes);
            }

            //return ""; 
        }

        /// <summary>
        /// Get a user with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppUserDto> GetAppUserAsync(Guid id)
        {
            var userDb = await _sucreUnitOfWork.repoSucreAppUser.FindAsync(id);
            if (userDb != null)
            {
                AppUserDto appUserTdo = new AppUserDto
                {
                    Id = userDb.Id,
                    Name = userDb.Name,
                    Description = userDb.Description,
                    Email = userDb.Email,
                    PasswordHash = userDb.PasswordHash,
                    GroupNumber = userDb.GroupNumber                    
                };
                return appUserTdo;
            }
            return null;
        }

        /// <summary>
        /// Get a user with email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUserDto> GetAppUserAsync(string email)
        {
            var userDb = await _sucreUnitOfWork.repoSucreAppUser.FirstOrDefaultAsync(user => user.Email == email);
            if (userDb != null)
            {
                AppUserDto appUserTdo = new AppUserDto
                {
                    Id = userDb.Id,
                    Name = userDb.Name,
                    Description = userDb.Description,
                    Email = userDb.Email,
                    PasswordHash = userDb.PasswordHash,
                    GroupNumber = userDb.GroupNumber
                };
                return appUserTdo;
            }
            return null;
        }

        public async Task<AppUserDto> GetAppUserByRefreshTokenAsync(Guid refreshToken)
        {
            try
            {
                var user = await _mediator.Send(
                new GetUserByRefreshTokenQuery()
                {
                    RefreshTokenId = refreshToken
                });
                var userDto = _appUserMapper.AppUserToAppUserDto(user);
                return userDto;
            }
            catch (Exception ex)
            {                
                LoggerExternal.LoggerEx.Error($"*->Error,UserService->{nameof(GetAppUserByRefreshTokenAsync)}: " +
                    $"Get user by refresh token, {ex.Message}");
                return null;
            }
            
        }

        /// <summary>
        /// Get a app user and assigned app role. Get app role not 
        /// assigned role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUsersAssigned"></param>
        /// <param name="appUsersNotAssigned"></param>
        /// <returns></returns>
        public AppUserDto GetAppUserRoles(
            Guid id,
            ref IEnumerable<AppRoleDto> appRolesAssigned,
            ref IEnumerable<AppRoleDto> appRolesNotAssigned)
        {
            try
            {
                //appUsersTdo = null;
                //var appRoleDb = _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
                //    filter: role => role.Id == id,
                //    includeProperties: $"{WC.AppUsersName}",
                //    isTracking: false).Result;
                var appUserDb = _sucreUnitOfWork.repoSucreAppUser.FirstOrDefault(
                    filter: user => user.Id == id,
                    includeProperties: $"{WC.AppRolesName}",
                    isTracking: false);

                if (appUserDb == null) { return null; }

                AppUserDto appUserTdo = new AppUserDto()
                {
                    Id = appUserDb.Id,
                    Name = appUserDb.Name,
                    Description = appUserDb.Description,
                    Email = appUserDb.Email,
                    PasswordHash = appUserDb.PasswordHash,
                    GroupNumber = appUserDb.GroupNumber
                };

                if (appUserDb.AppRoles.Count() == 0)
                {
                    appRolesAssigned = new HashSet<AppRoleDto>();
                }
                else
                {
                    appRolesAssigned = new HashSet<AppRoleDto>();
                    appRolesAssigned = appUserDb.AppRoles
                    .Select(role => new AppRoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Value = role.Value
                    });
                }

                List<Guid> listIdAppRole = new List<Guid>();

                listIdAppRole = appRolesAssigned
                    .Select(role => role.Id).ToList();

                Expression<Func<AppRole, bool>> epFilter = null;

                if (listIdAppRole.Count == 0)
                {
                    //var userDb0 = _sucreUnitOfWork.repoSucreAppUser.GetAll(isTracking: false);
                }
                else
                {
                    bool begId = true;
                    foreach (var idRole in listIdAppRole)
                    {
                        if (begId)
                        {
                            epFilter = item => item.Id != idRole;
                            begId = false;
                        }
                        else
                        {
                            epFilter = epFilter.And(item => item.Id != idRole);
                        }
                    }
                };

                var appRoleDb = _sucreUnitOfWork.repoSucreAppRole.GetAll(
                    filter: epFilter,
                    isTracking: false);
                appRolesNotAssigned = appRoleDb
                    .Select(role => new AppRoleDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Value = role.Value
                    });

                return appUserTdo;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error when getting a user with roles");
                return null;
            }
        }

        /// <summary>
        /// Get list users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AppUserDto>> GetListUsersAsync()
        {
            try
            {
                var usersDb = await _sucreUnitOfWork.repoSucreAppUser.GetAllAsync(
                includeProperties: $"{WC.AppRolesName}",
                isTracking: false);
                IEnumerable<AppUserDto> appUsers = usersDb
                    .Select(user => new AppUserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Description = user.Description,
                        Email = user.Email,
                        PasswordHash = user.PasswordHash,
                        GroupNumber = user.GroupNumber,
                    });
                return appUsers;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Get list users");
            }
            return null;
        }

        /// <summary>
        /// Get string id group
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<string> GetStringGropId(string email)
        {
            
            int groupNumber = (await _sucreUnitOfWork.repoSucreAppUser
                .FirstOrDefaultAsync(user => user.Email == email)).GroupNumber;
            
            return groupNumber.ToString();
        }
        /// <summary>
        /// Get string name role 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<string> GetStringTypeRole(string email)
        {
            var roles = await _sucreUnitOfWork.repoSucreAppUser
                .FirstOrDefaultAsync(
                filter: user => user.Email == email,
                includeProperties: WC.AppRolesName,
                isTracking:false);
            if (roles != null)
            {
                if (roles.AppRoles.Count() == 0)
                    return "";
                else
                    return roles.AppRoles.ToList()[0].Name;
            }
            return "";
           
            //var roles1 = DelListUser
            //    .ListUser
            //    .FirstOrDefault(item => item.Email == email)
            //    .AppRoles
            //    .ToList();
            ////var f = d.AppRoles.ToList();
            //if (roles !=null && roles1.Count != 0) 
            //{
            //    string nameRole = roles1[0].Name;
            //    return nameRole;
            //}
            //return "";
            
        }
        /// <summary>
        /// Full user name as a string
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetStringAppUserAsync(Guid Id)
        {
            var str = _sucreUnitOfWork.repoSucreAppUser
                .GetStringName(_sucreUnitOfWork.repoSucreAppUser
                .Find(Id));
            return str;
        }

        /// <summary>
        /// Password verification
        /// </summary>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsPasswordCorrect(string password, string email = null)
        {
            var passwordHash = GenerateMD5Hash(password);
            bool result = false;
            if (email != null)
            {
                result = _sucreUnitOfWork.repoSucreAppUser
                    .GetAsQueryable()
                    .Any(item => (item.Email == email && item.PasswordHash == passwordHash));
                return result;
            }
            result = _sucreUnitOfWork.repoSucreAppUser.GetAsQueryable().Any(item => item.PasswordHash == passwordHash);
            return result;            
        }

        /// <summary>
        /// User verification
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExist(string email)
        {
            if (await _sucreUnitOfWork.repoSucreAppUser.Count() == 0) return false;

            bool result = _sucreUnitOfWork.repoSucreAppUser
                .GetAsQueryable()
                .Any(item => item.Email == email);
            //var result3 = await _sucreUnitOfWork.repoSucreAppUser
            //    .FirstOrDefaultAsync(item => item.Email == email);
            return result;

            

            //bool result = await _sucreUnitOfWork.repoSucreAppUser
            //    .GetAsQueryable().ToList()
            //    .Any(item => item.Email == email);

            if (DelListUser.ListUser.Count == 0) return false;
            //var hh=DelListUser.LzistUser.Find(item => item.Email == email).Email.Any();
            bool result2 = DelListUser.ListUser.Any(item => item.Email == email);
            return true; // result;
            //if (result)//(DelListUser.ListUser.FirstOrDefault(item => item.Email == email).Email.Any())
            //{
            //    return true;
            //}
            //return false;
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="userTdo"></param>
        /// <returns></returns>
        public async Task<int> RegisterUserAsync(AppUserRegTdo userDto)
        {
            //int id = DelListUser.ListUser.Max(i => i.Id);

            //int grm = userTdo.GroupId;

            var userReg = new AppUserDto()
            {
                //Id = Guid.NewGuid(),
                Email = userDto.Email,
                PasswordHash = GenerateMD5Hash(userDto.Password),
                GroupNumber = userDto.GroupId,
            };

            if (!await UpsertUserAsync(userReg)) return 0;

            //AppUser userAdd = new AppUser
            //{
            //    Id=userReg.Id,
            //    Email = userReg.Email,
            //    PasswordHash = userReg.PasswordHash,
            //    GroupId = userReg.GroupId,
            //    Name = userReg.Name,
            //    Description = userReg.Description
            //};

            //DelListUser.AddInListUser(userAdd);
            return 1;
            
        }
               
        public string getmd5hash(string sss)
        {
            return GenerateMD5Hash(sss);
        }

        /// <summary>
        /// Removing/adding a role associated with a user
        /// </summary>
        /// <param name="idUser">id user</param>
        /// <param name="idRole">id role</param>
        /// <param name="action">action performed on the user</param>
        /// <returns></returns>
        public async Task<bool> ListAppUserRolesActionAsync(Guid idUser, Guid idRole, ActionRoleUser action = ActionRoleUser.None)
        {   
            try
            {
                AppUser appUser = await _sucreUnitOfWork.repoSucreAppUser
                    .FirstOrDefaultAsync(filter: user => user.Id == idUser,
                                        includeProperties: WC.AppRolesName);
                if (action == ActionRoleUser.Delete)
                {
                    AppRole appRole = appUser.AppRoles
                        .FirstOrDefault(role => role.Id == idRole);
                    appUser.AppRoles.Remove(appRole);
                }
                if (action == ActionRoleUser.Add)
                {                    
                    AppRole addAppRole = await _sucreUnitOfWork.repoSucreAppRole
                        .FirstOrDefaultAsync(filter: role => role.Id == idRole,
                                            isTracking: false);
                    appUser.AppRoles.Add(addAppRole);
                }
                if (action != ActionRoleUser.None)
                    await _sucreUnitOfWork.CommitAsync();
                else
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Error when deleting/updating a roles associated with a user");
                return false;
            }

        }

        /// <summary>
        /// Creating or updating user
        /// </summary>
        /// <param name="appUserTdo"></param>
        /// <returns></returns>
        public async Task<bool> UpsertUserAsync(AppUserDto appUserDto)
        {
            try
            {
                
                AppUser appUser = new AppUser()
                {
                    Name = appUserDto.Name,
                    Description= appUserDto.Description,
                    Email = appUserDto.Email,
                    PasswordHash= appUserDto.PasswordHash,
                    
                    //GroupId = 0,
                    //GroupUser = null
                };
                if (appUserDto.Id == null || appUserDto.Id == Guid.Empty)
                {
                    
                    appUser.Id = Guid.NewGuid();
                    appUser.GroupNumber  = 99;
                    await _sucreUnitOfWork.repoSucreAppUser.AddAsync(appUser);
                    //return true;
                }
                else
                {
                    appUser.GroupNumber = appUserDto.GroupNumber;
                    
                    //var userDb = await _sucreUnitOfWork.repoSucreAppUser.FirstOrDefaultAsync(
                    //    filter: user => user.Id == appUserTdo.Id,
                    //    isTracking: false);
                    //if (userDb == null)
                    //{
                    //    return false;
                    //}
                    //else
                    ////if (roleDb != null)
                    //{
                        appUser.Id = appUserDto.Id;
                        //appRoleTdo.Id = roleDb.Id;
                        //roleDb.Name = appRoleTdo.Name;
                        //roleDb.Value = appRoleTdo.Value;
                        _sucreUnitOfWork.repoSucreAppUser.Update(appUser);
                        //return true;
                    //}
                }
                _sucreUnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Upsert User");
                return false;
            }
        }
    }
}
