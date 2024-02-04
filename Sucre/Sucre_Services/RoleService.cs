using LinqKit;
using Sucre_Core;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Linq.Expressions;
using Sucre_Core.DTOs;
using MediatR;
using System.Reflection.Metadata.Ecma335;
using Sucre_DataAccess.CQS.Queries;

namespace Sucre_Services
{
    public class RoleService : IRoleService
    {
        private readonly IMediator _mediator;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;

        public RoleService(
            IMediator mediator,
            ISucreUnitOfWork sucreUnitOfWork)
        {
            _mediator = mediator;
            _sucreUnitOfWork = sucreUnitOfWork;
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="appRoleTdo"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRoleAsync(AppRoleDto appRoleDto)
        {
            try
            {
                await _sucreUnitOfWork.repoSucreAppRole.RemoveByIdAsync(appRoleDto.Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error when deleting role");
                return false;
            }
        }

        /// <summary>
        /// Get a role with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppRoleDto> GetAppRoleAsync(Guid id)
        {
            var roleDb = await _sucreUnitOfWork.repoSucreAppRole.FindAsync(id);
            if (roleDb != null)
            {
                AppRoleDto appRoleTdo = new AppRoleDto
                {
                    Id = roleDb.Id,
                    Name = roleDb.Name,
                    Value = roleDb.Value
                };
                return appRoleTdo;
            }
            return null;
        }

        /// <summary>
        /// Get a app role and assigned app user. Get app user not assigned role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUsersAssigned"></param>
        /// <param name="appUsersNotAssigned"></param>
        /// <returns></returns>
        public AppRoleDto GetAppRoleUsers(Guid id,
            ref IEnumerable<AppUserDto> appUsersAssigned,
            ref IEnumerable<AppUserDto> appUsersNotAssigned)
        {
            try
            {
                //appUsersTdo = null;
                //var appRoleDb = _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
                //    filter: role => role.Id == id,
                //    includeProperties: $"{WC.AppUsersName}",
                //    isTracking: false).Result;
                var appRoleDb = _sucreUnitOfWork.repoSucreAppRole.FirstOrDefault(
                    filter: role => role.Id == id,
                    includeProperties: $"{WC.AppUsersName}",
                    isTracking: false);

                if (appRoleDb == null) { return null; }

                AppRoleDto appRoleDto = new AppRoleDto()
                {
                    Id = appRoleDb.Id,
                    Name = appRoleDb.Name,
                    Value = appRoleDb.Value
                };

                if (appRoleDb.AppUsers.Count() == 0)
                {
                    appUsersAssigned = new HashSet<AppUserDto>();
                }
                else
                {
                    appUsersAssigned = new HashSet<AppUserDto>();
                    appUsersAssigned = appRoleDb.AppUsers
                    .Select(user => new AppUserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Description = user.Description,
                        Email = user.Email,
                        PasswordHash = user.PasswordHash,
                        GroupNumber = user.GroupNumber
                    });
                }
                
                List<Guid> listIdAppUser = new List<Guid>();

                listIdAppUser = appUsersAssigned
                    .Select(user => user.Id).ToList();

                Expression<Func<AppUser, bool>> epFilter = null;

                if (listIdAppUser.Count == 0)
                {
                    //var userDb0 = _sucreUnitOfWork.repoSucreAppUser.GetAll(isTracking: false);
                }
                else
                {
                    bool begId = true;
                    foreach (var idUser in listIdAppUser)
                    {
                        if (begId)
                        {
                            epFilter = item => item.Id != idUser;
                            begId = false;
                        }
                        else
                        {
                            epFilter = epFilter.And(item => item.Id != idUser);
                        }
                    }
                };

                var appUserDb = _sucreUnitOfWork.repoSucreAppUser.GetAll(
                    filter: epFilter,
                    isTracking: false);
                appUsersNotAssigned = appUserDb
                    .Select(user => new AppUserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Description = user.Description,
                        Email = user.Email,
                        PasswordHash = user.PasswordHash,
                        GroupNumber = user.GroupNumber
                    });

                return appRoleDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error when getting a role with users");
                return null;
            }
        }

        /// <summary>
        /// Get list of roles
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AppRoleDto>> GetListRolesAsync()
        {

            try
            {
                var rolesDb = await _sucreUnitOfWork.repoSucreAppRole.GetAllAsync(
                includeProperties: $"{WC.AppUsersName}",
                isTracking: false);
                IEnumerable<AppRoleDto> appRoles = rolesDb.Select(role => new AppRoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Value = role.Value
                });
                return appRoles;                
            }
            catch (Exception ex) 
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Get list roles");
            }
            return null;
        }

        public async Task<IEnumerable<AppRoleDto>> GetListRolesByUserIdAsync(Guid id)
        {
            try
            {
                GetRolesByUserIdQuery query = new GetRolesByUserIdQuery() { Id = id };
                var result = await _mediator.Send(query);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        /// <summary>
        /// Removing/adding a user associated with a role
        /// </summary>
        /// <param name="idRole">id role</param>
        /// <param name="idUser">id user</param>
        /// <param name="action">action performed on the role</param>
        /// <returns></returns>
        public async Task<bool> ListAppRoleUsersActionAsync(Guid idRole, Guid idUser, ActionRoleUser action = ActionRoleUser.None)
        {
            try
            {
                AppRole appRole = await _sucreUnitOfWork.repoSucreAppRole
                    .FirstOrDefaultAsync(filter: role => role.Id == idRole,
                                        includeProperties: WC.AppUsersName);
                if (action == ActionRoleUser.Delete)
                {
                    AppUser appUser = appRole.AppUsers
                        .FirstOrDefault(user => user.Id == idUser);
                    appRole.AppUsers.Remove(appUser);
                }
                if (action == ActionRoleUser.Add)
                {                    
                    AppUser addAppUser = await _sucreUnitOfWork.repoSucreAppUser
                        .FirstOrDefaultAsync(filter: user => user.Id == idUser,
                                            isTracking: false);
                    appRole.AppUsers.Add(addAppUser);
                }
                if (action != ActionRoleUser.None)
                    await _sucreUnitOfWork.CommitAsync();
                else 
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Error when deleting/updating a user associated with a role");
                return false;
            }

        }

        /// <summary>
        /// creating or updating role
        /// </summary>
        /// <param name="appRoleTdo">role object</param>
        /// <returns></returns>
        public async Task<bool> UpsertRoleAsync(AppRoleDto appRoleDto)
        {
            try
            {
                AppRole appRole = new AppRole()
                {
                    Name = appRoleDto.Name,
                    Value = appRoleDto.Value.ToString().ToUpper()
                };
                if (appRoleDto.Id == null || appRoleDto.Id == Guid.Empty)
                {
                    appRole.Id = Guid.NewGuid();
                    await _sucreUnitOfWork.repoSucreAppRole.AddAsync(appRole);
                    //return true;
                }
                else
                {
                    var roleDb = await _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
                        filter: role => role.Id == appRoleDto.Id,
                        isTracking: false);
                    if (roleDb == null)
                    {
                        return false;
                    }
                    else
                    //if (roleDb != null)
                    {
                        appRole.Id = appRoleDto.Id;
                        //appRoleTdo.Id = roleDb.Id;
                        //roleDb.Name = appRoleTdo.Name;
                        //roleDb.Value = appRoleTdo.Value;
                        _sucreUnitOfWork.repoSucreAppRole.Update(appRole);
                        //return true;
                    }                    
                }
                _sucreUnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error: Upsert Role");
                return false;                
            }            
        }        
        

    }
}
