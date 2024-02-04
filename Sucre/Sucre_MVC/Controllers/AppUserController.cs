using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Data;
using System.Security.Claims;
using FluentValidation;

namespace Sucre_MVC.Controllers
{
    [Authorize]
    public class AppUserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IValidator<AppUserLoginM> _loginValidator;

        public AppUserController(
            IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            IRoleService roleService,
            IUserService userService,
            IValidator<AppUserLoginM> loginValidator)
        {
            _roleService = roleService;
            _userService = userService;
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _loginValidator = loginValidator;
        }
               

        //[Authorize(Policy = WC.SupervisorPolicy)]

        #region App Roles
        /// <summary>
        /// List app roles
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> ListAppRole()
        {            
            var appRolesTdo = await _roleService.GetListRolesAsync();
            IEnumerable<AppRoleM> appRolesM = appRolesTdo
                .Select(role => new AppRoleM
                {
                    Id = role.Id,
                    Name = role.Name,
                    Value = role.Value
                });
            return View(appRolesM);
        }

        /// <summary>
        /// Create or edit app roles
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [HttpGet]
        public async Task<IActionResult> UpsertAppRole(Guid? Id)
        {
            //TempData["FieldIsEmpty"] = null;
            AppRoleM appRoleM = new AppRoleM();
            if (User.IsInRole(WC.SupervisorRole)) 
                appRoleM.IsEdit = true;
            else
                appRoleM.IsEdit = false;
            //cexM cexM = new CexM();
            
            if (Id == null)
            {
                return View(appRoleM);
            }
            else
            {
                var appRoleTdo = await _roleService.GetAppRoleAsync(Id.Value);
                
                if (appRoleTdo != null)
                {
                    appRoleM.Id = appRoleTdo.Id;
                    appRoleM.Name = appRoleTdo.Name;
                    appRoleM.Value = appRoleTdo.Value;
                    return View(appRoleM);
                    
                }
                else
                {
                    return NotFound($"Search for role with Id = {Id.Value} failed");
                }
            }
        }
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpsertAppRole(AppRoleM appRoleM)
        {
            //TempData["FieldIsEmpty"] = null;
            if (ModelState.IsValid)
            { 
                AppRoleDto appRoleTdo = new AppRoleDto();                
                if (appRoleM.Id == null || appRoleM.Id == Guid.Empty)
                {
                    appRoleTdo.Name = appRoleM.Name;
                    appRoleTdo.Value = appRoleM.Value;
                    //await _sucreUnitOfWork.repoSucreCex.AddAsync(cex);
                }
                else
                {
                    //Update
                    appRoleTdo.Id = appRoleM.Id;
                    appRoleTdo.Name = appRoleM.Name;
                    appRoleTdo.Value = appRoleM.Value;
                }
                bool commitRole = await _roleService.UpsertRoleAsync(appRoleTdo);
                if (commitRole)
                    return RedirectToAction(nameof(ListAppRole));
            }
            return View(appRoleM);
        }
        
        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        public async Task<IActionResult> DeleteAppRole(Guid? Id)
        {
            if (Id == null || Id == Guid.Empty) return NotFound("ID is not specified or it is empty");
            var appRoleTdo =await  _roleService.GetAppRoleAsync(Id.Value);            
            if (appRoleTdo == null) return NotFound($"Search for role with Id = {Id.Value} failed");
            AppRoleM appRoleM = new AppRoleM
            {
                Id = appRoleTdo.Id,
                Name = appRoleTdo.Name,
                Value = appRoleTdo.Value
            };            
            return View(appRoleM);
        }
        [HttpPost]
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAppRole")]
        public async Task<IActionResult> DeleteAppRolePost(Guid? Id)
        {
            if (Id == null || Id == Guid.Empty) return NotFound("Id is not specified or it is empty");
            var appRoleTdo = await _roleService.GetAppRoleAsync(Id.Value);
            if (appRoleTdo == null) return NotFound($"Search for role with Id = {Id.Value} failed");
            bool commitRole = await _roleService.DeleteRoleAsync(appRoleTdo);
            if (commitRole)
                return RedirectToAction(nameof(ListAppRole));            
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [HttpGet]
        public async Task<IActionResult> ListAppRoleUsers(Guid? Id)
        {
            //var pointCanalesDb = _sukreUnitOfWork.repoSukrePoint.GetAll(filter: item => item.Id == Id.Value,
            //                                                         includeProperties: WC.CanalsName);
            if (Id == null || Id.Value == Guid.Empty)
                return BadRequest("Id is not specified or it is empty");

            IEnumerable<AppUserDto> appRoleUsersAssigned = new HashSet<AppUserDto>();
            IEnumerable<AppUserDto> appRoleUsersNotAssigned = new HashSet<AppUserDto>();

            AppRoleDto appRoleDto = _roleService.GetAppRoleUsers(Id.Value,
                ref appRoleUsersAssigned,
                ref appRoleUsersNotAssigned);

            if (appRoleDto == null) return BadRequest($"Role Id={Id.Value} bad or failer!!!"); View();

            AppRoleUsersM appRoleUsersM = new AppRoleUsersM
            {
                Id = appRoleDto.Id,
                Name = appRoleDto.Name                
            };


            foreach (var usr in  appRoleUsersAssigned)
            {
                appRoleUsersM.AppUsersM.Add(new AppUserM
                {
                    Id = usr.Id,
                    Name = usr.Name,
                    Description = usr.Description,
                    Email = usr.Email,
                    PasswordHash = usr.PasswordHash,
                    GroupNumber = usr.GroupNumber,
                });
            };

            List<SelectListItem> dd = new List<SelectListItem>();
            if (appRoleUsersM.Name == "Supervisor" && !User.IsInRole(WC.SupervisorRole))
                appRoleUsersM.FreeAppUsersSelectList = null;
            else
            {
                appRoleUsersM.FreeAppUsersSelectList = appRoleUsersNotAssigned
                    .Select(usr => new SelectListItem
                    {
                        Text = $"{usr.Name},{usr.Email},{usr.GroupNumber.ToString()}",
                        Value = usr.Id.ToString(),
                    });
            }
            

            return View(appRoleUsersM);

            
        }
        [HttpPost]
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAppRoleUsersDelete(Guid Id, Guid IdAppUser )
        {
            //var id = Id;
            //var idc = IdCannale;            
            //AppRole appRole = await _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
            //    filter: role => role.Id == Id,
            //    includeProperties: WC.AppUsersName);
            //AppUser appUser = appRole.AppUsers.FirstOrDefault(
            //    user => user.Id == IdAppUser );
            //appRole.AppUsers.Remove( appUser ); 
            //await _sucreUnitOfWork.CommitAsync();
            if (await _roleService.ListAppRoleUsersActionAsync(Id, IdAppUser, ActionRoleUser.Delete))
                return RedirectToAction(nameof(ListAppRoleUsers), new { Id = Id} );            
            else
            {
                LoggerExternal.LoggerEx.Error($"***->AppUserController->{nameof(ListAppRoleUsersDelete).ToString()}: " +
                    $"Error when deleting linked user Id={IdAppUser} to role Id={Id}");
                return BadRequest($"Error when deleting linked user Id={IdAppUser} to role Id={Id}");
            }
                
            
        }
        [HttpPost]
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAppRoleUsersAdding(Guid Id, Guid Add, AppRoleUsersM appRoleUsersM)
        {

            if (appRoleUsersM.AddUser == Guid.Empty)
                return RedirectToAction(nameof(ListAppRoleUsers), new { Id = Id });
            Guid AddIdUser = appRoleUsersM.AddUser;

            //var res = HttpContext;
            //var id = Id;
            //var idc = Add;

            //AppRole appRole = await _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
            //    filter: role => role.Id == Id,
            //    includeProperties: WC.AppUsersName);
            //AppUser addAppUser = await _sucreUnitOfWork.repoSucreAppUser.FirstOrDefaultAsync(
            //    filter: user => user.Id == AddIdUser,
            //    isTracking: false);
            //appRole.AppUsers.Add(addAppUser);
            //await _sucreUnitOfWork.CommitAsync();

            if (await _roleService.ListAppRoleUsersActionAsync(Id, AddIdUser, ActionRoleUser.Add))
                return RedirectToAction(nameof(ListAppRoleUsers), new { Id = Id });
            else
            {
                LoggerExternal.LoggerEx.Error($"***->AppUserController->{nameof(ListAppRoleUsersAdding).ToString()}: " +
                    $"Error when adding linked user Id={AddIdUser} to role Id={Id}");
                return BadRequest($"Error when adding linked user Id={AddIdUser} to role Id={Id}");
            }

        }
        #endregion

        #region App Users
        /// <summary>
        /// List app users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> ListAppUser()
        {
            var appUsersTdo = await _userService.GetListUsersAsync();
            IEnumerable<AppUserM> appUsersM = appUsersTdo
                .Select(user => new AppUserM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Description = user.Description,
                    PasswordHash = user.PasswordHash,
                    GroupNumber = user.GroupNumber,           
                });
            return View(appUsersM);
        }

        /// <summary>
        /// Create or edit app user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [HttpGet]
        public async Task<IActionResult> UpsertAppUser(Guid? Id)
        {
            //TempData["FieldIsEmpty"] = null;
            AppUserM appUserM = new AppUserM();
            if (User.IsInRole(WC.SupervisorRole))
                appUserM.IsEdit = true;
            else
                appUserM.IsEdit = false;
            //cexM cexM = new CexM();

            if (Id == null) {
                return View(appUserM);
            }
            else
            {
                var appUserDto = await _userService.GetAppUserAsync(Id.Value);

                if (appUserDto != null)
                {
                    appUserM.Id = appUserDto.Id;
                    appUserM.Name = appUserDto.Name;
                    appUserM.Description = appUserDto.Description;
                    appUserM.Email = appUserDto.Email;
                    appUserM.PasswordHash = appUserDto.PasswordHash;
                    appUserM.GroupNumber = appUserDto.GroupNumber;
                    return View(appUserM);
                }
                else
                {
                    return NotFound($"Search for user with Id = {Id.Value} failed");
                }
            }
        }
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UpsertAppUser(AppUserM appUserM)
        {
            //TempData["FieldIsEmpty"] = null;
            if (ModelState.IsValid)
            {
                AppUserDto appUserDto = new AppUserDto();
                if (appUserM.Id == null || appUserM.Id == Guid.Empty)
                {
                    appUserDto.Name = appUserM.Name;
                    appUserDto.Description = appUserM.Description;
                    appUserDto.Email = appUserM.Email;
                    appUserDto.PasswordHash = WM.GenerateMD5Hash(
                        appUserM.PasswordHash,
                        _configuration["AppSettings:PasswordSalt"]);
                    appUserDto.GroupNumber = appUserM.GroupNumber;
                    //await _sucreUnitOfWork.repoSucreCex.AddAsync(cex);
                }
                else
                {
                    //Update
                    appUserDto.Id = appUserM.Id;
                    appUserDto.Name = appUserM.Name;
                    appUserDto.Description = appUserM.Description;
                    appUserDto.Email = appUserM.Email;
                    appUserDto.PasswordHash = appUserM.PasswordHash;
                    appUserDto.GroupNumber = appUserM.GroupNumber;
                }
                bool commitRole = await _userService.UpsertUserAsync(appUserDto);
                if (commitRole)
                    return RedirectToAction(nameof(ListAppUser));
            }
            return View(appUserM);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        public async Task<IActionResult> DeleteAppUser(Guid? Id)
        {
            if (Id == null || Id == Guid.Empty) return NotFound("ID is not specified or it is empty");
            var strNameUser = _sucreUnitOfWork.repoSucreAppUser.GetStringName(
               await _sucreUnitOfWork.repoSucreAppUser.FindAsync(Id.Value)) ;// await _userService.GetAppUserAsync(Id.Value);
            AppUserDelM appUserDelM = new AppUserDelM
            {

                Id = Id.Value,
                NameUser = strNameUser
            };
            return View(appUserDelM);
        }
        [HttpPost]
        [Authorize(Roles = $"{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAppUser")]
        public async Task<IActionResult> DeleteAppUserPost(Guid? Id)
        {
            if (Id == null || Id == Guid.Empty) return NotFound("Id is not specified or it is empty");
            bool commitRole = await _userService.DeleteUserIdAsync(Id.Value);
            if (commitRole)
                return RedirectToAction(nameof(ListAppUser));
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [HttpGet]
        public async Task<IActionResult> ListAppUserRoles(Guid? Id)
        {            
            if (Id == null || Id.Value == Guid.Empty)
                return BadRequest("Id is not specified or it is empty");

            IEnumerable<AppRoleDto> appUserRolesAssigned = new HashSet<AppRoleDto>();
            IEnumerable<AppRoleDto> appUserRolesNotAssigned = new HashSet<AppRoleDto>();

            AppUserDto appUserDto = _userService.GetAppUserRoles(
                Id.Value,
                ref appUserRolesAssigned,
                ref appUserRolesNotAssigned);

            if (appUserDto == null) return BadRequest($"User Id={Id.Value} bad or failer!!!"); 

            AppUserRolesM appUserRolesM = new AppUserRolesM
            {
                Id = Id.Value,
                Name = appUserDto.Name,
                Email = appUserDto.Email,
            };

            foreach (var usr in appUserRolesAssigned)
            {
                appUserRolesM.AppRolesM.Add(new AppRoleM
                {
                    Id = usr.Id,
                    Name = usr.Name,
                    Value = usr.Value
                });
            };

            List<SelectListItem> tmpSLI = new List<SelectListItem>();

            //appUserRolesM.FreeAppRolesSelectList = new List<SelectListItem>();
            
            foreach (var role in appUserRolesNotAssigned)
            {
                if (role.Name == "Supervisor")
                {
                    if (User.IsInRole(WC.SupervisorRole))
                    {
                        tmpSLI.Add(new SelectListItem
                        {
                            Text = $"{role.Name},{role.Value}",
                            Value = role.Id.ToString(),
                        });
                    }
                }
                else
                {
                    tmpSLI.Add(new SelectListItem
                    {
                        Text = $"{role.Name},{role.Value}",
                        Value = role.Id.ToString(),
                    });
                }
            }
            //appUserRolesM.FreeAppRolesSelectList = appUserRolesNotAssigned
            //    .Select(role => new SelectListItem
            //    {

            //        Text = $"{role.Name},{role.Value}",
            //        Value = role.Id.ToString(),
            //    });

            appUserRolesM.FreeAppRolesSelectList = tmpSLI;

            return View(appUserRolesM);
        }
        [HttpPost]        
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAppUserRolesDelete(Guid Id, Guid IdAppRole)
        {
            //var id = Id;
            //var idc = IdCannale;            
            //AppUser appUser = await _sucreUnitOfWork.repoSucreAppUser.FirstOrDefaultAsync(
            //    filter: user => user.Id == Id,
            //    includeProperties: WC.AppRolesName);
            //AppRole appRole = appUser.AppRoles.FirstOrDefault(
            //    user => user.Id == IdAppRole);
            //appUser.AppRoles.Remove(appRole);
            //await _sucreUnitOfWork.CommitAsync();
            //return RedirectToAction(nameof(ListAppUserRoles), new { Id = Id });

            if (await _userService.ListAppUserRolesActionAsync(Id, IdAppRole, ActionRoleUser.Delete))
                return RedirectToAction(nameof(ListAppUserRoles), new { Id = Id });
            else
            {
                LoggerExternal.LoggerEx.Error($"***->AppUserController->{nameof(ListAppUserRolesDelete).ToString()}: " +
                    $"Error when deleting linked role Id={IdAppRole} to user Id={Id}");
                return BadRequest($"Error when deleting linked role Id={IdAppRole} to user Id={Id}");
            }
        }
        [HttpPost]        
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAppUserRolesAdding(Guid Id, Guid Add, AppUserRolesM appUserRolesM)
        {

            if (appUserRolesM.AddRole == Guid.Empty)
                return RedirectToAction(nameof(ListAppUserRoles), new { Id = Id });
            Guid AddIdRole = appUserRolesM.AddRole;

            //var res = HttpContext;
            //var id = Id;
            //var idc = Add;

            //AppUser appUser = await _sucreUnitOfWork.repoSucreAppUser.FirstOrDefaultAsync(
            //    filter: user => user.Id == Id,
            //    includeProperties: WC.AppRolesName);
            //AppRole addAppRole = await _sucreUnitOfWork.repoSucreAppRole.FirstOrDefaultAsync(
            //    filter: role => role.Id == AddIdRole,
            //    isTracking: false);
            //appUser.AppRoles.Add(addAppRole);
            //await _sucreUnitOfWork.CommitAsync();

            //return RedirectToAction(nameof(ListAppUserRoles), new { Id = Id });

            if (await _userService.ListAppUserRolesActionAsync(Id, AddIdRole, ActionRoleUser.Add))
                return RedirectToAction(nameof(ListAppUserRoles), new { Id = Id });
            else
            {
                LoggerExternal.LoggerEx.Error($"***->AppUserController->{nameof(ListAppUserRolesAdding).ToString()}: " +
                    $"Error when adding linked role Id={AddIdRole} to user Id={Id}");
                return BadRequest($"Error when adding linked role Id={AddIdRole} to user Id={Id}");
            }
        }
        #endregion

        #region Authenticate
        public IActionResult AccessDeniedPath()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AppUserRegister(string returnUrl)
        {
            //var model = new AppUserRegisterM();
            //if (modelIn != null)
            //{
            //    model.Email = modelIn.Email;
            //    model.Password = modelIn.Password;
            //    model.PasswordConfirmation = modelIn.PasswordConfirmation;
            //}
            
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("AppUserRegister")]
        public async Task<IActionResult> AppUserRegisterPost(AppUserRegisterM model)
        {

        //    if (model.Password.IsNullOrEmpty()) 
        //        { ModelState.AddModelError("Password", "Password null"); };
            if (ModelState.IsValid) 
            {                
                if (await _userService.IsUserExist(model.Email) || IsSuperUser(email: model.Email, checkName: true))
                {
                    //var gg = _userService.IsUserExist(model.Email);
                    ModelState.AddModelError("Email", "Email is used");                    
                }
                else 
                {                    
                    var dto = new AppUserRegTdo()
                    {
                        Email = model.Email,
                        Password = model.Password
                    };
                    if (await _userService.RegisterUserAsync(dto) == 0)
                        return View(model);

                    //var ht = HttpContext.User.Identity.IsAuthenticated;

                    var cpl = new ClaimsPrincipal(await _userService.Authenticate(dto.Email));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        cpl);

                    var dd = User.IsInRole(WC.UserRole);
                    var ddd = User.Identity.IsAuthenticated;

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    //    new ClaimsPrincipal( await _userService.Authenticate(dto.Email)));
                    //var dd = User.Identity.IsAuthenticated;

                    return RedirectToAction("Index", "Home");
                    //ModelState.AddModelError("Email", "tratataaaa");
                };
                
            }
            //else 
            //{
                AppUserRegisterM modelOut = new AppUserRegisterM
                {
                    Email = model.Email,
                    Password = model.Password,
                    PasswordConfirmation = model.PasswordConfirmation
                };
                return View(modelOut);
            //}
            //return View(model);
            
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AppUserLogin(string? returnUrl)        
        {
           
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("AppUserLogin")]
        public async Task<IActionResult> AppUserLoginPost(AppUserLoginM model)
        {
            bool authenticateRun = false;
            bool super = false;
            if (ModelState.IsValid)
            {
                //string user = model.Email.Substring(0,model.Email.IndexOf("@"));
                //bool superUser = model.Email.Substring(0, model.Email.IndexOf("@")) == 
                //    _configuration["AppSettings:SuperUser"]?true:false;
                //bool superPassword = 
                //    WM.GenerateMD5Hash(model.Password, _configuration["AppSettings:PasswordSalt"].ToString()) ==
                //    _configuration["AppSettings:SuperPassword"].ToString()?true:false;

                super = IsSuperUser(model.Email, model.Password);

                if (super) 
                {
                    authenticateRun = true;
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (await _userService.IsUserExist(model.Email))
                    {
                        //var gg = _userService.getmd5hash(model.Password);
                        bool passwordCorrect = await _userService.IsPasswordCorrect(model.Password);
                        //var gg = _userService.IsUserExist(model.Email);

                        if (passwordCorrect)
                        {
                            authenticateRun = true;
                            //var claimPincipal = new ClaimsPrincipal(await _userService.Authenticate(model.Email));
                            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            //    claimPincipal);
                            //return RedirectToAction("Index", "Home");

                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Password is not correct");
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("Email", "Email is not found");
                        //ModelState.AddModelError("Email", "tratataaaa");
                    };
                }
            }
            
            if (authenticateRun)
            {
                var claimPincipal = new ClaimsPrincipal(await _userService.Authenticate(model.Email, super));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    claimPincipal);
                return RedirectToAction("Index", "Home");
            }

            AppUserLoginM modelOut = new AppUserLoginM
            {
                Email = model.Email,
                Password = model.Password,
                ReturnUrl = model.ReturnUrl,
                //PasswordConfirmation = model.PasswordConfirmation
            };
            return View(modelOut);
            //}
            //return View(model);

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AppUserLoginJs([FromBody]AppUserLoginM model)
        {
            int StatusCode = 470;
            try
            {
                bool authenticateRun = false;
                bool super = false;
                
                if (HttpContext.Response.Headers["Login-Js"].ToString() != null)
                {
                    HttpContext.Response.Headers.Remove("Login-Js");
                }
                var resultCustomValidate = await _loginValidator.ValidateAsync(model);
                if (resultCustomValidate.IsValid)
                {   
                    if (ModelState.IsValid)
                    {                        
                        super = IsSuperUser(model.Email, model.Password);

                        if (super)
                        {
                            authenticateRun = true;
                            //return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (await _userService.IsUserExist(model.Email))
                            {
                                //var gg = _userService.getmd5hash(model.Password);
                                bool passwordCorrect = await _userService.IsPasswordCorrect(model.Password, model.Email);
                                //var gg = _userService.IsUserExist(model.Email);

                                if (passwordCorrect)
                                {
                                    authenticateRun = true;
                                    //var claimPincipal = new ClaimsPrincipal(await _userService.Authenticate(model.Email));
                                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                    //    claimPincipal);
                                    //return RedirectToAction("Index", "Home");

                                }
                                else
                                {
                                    StatusCode = 474;
                                    HttpContext.Response.Headers.Add("Login-Js", $"Warning, the password for the user {model.Email} is not correct (474)");
                                    ModelState.AddModelError("Password", "Password is not correct");
                                    LoggerExternal.LoggerEx.Warning($"*-->Warning, the password for the user {model.Email} is not correct");
                                }
                            }
                            else
                            {
                                StatusCode = 473;
                                HttpContext.Response.Headers.Add("Login-Js", $"Warning, email {model.Email} not found (473)");
                                ModelState.AddModelError("Email", "Email not found");
                                LoggerExternal.LoggerEx.Warning($"*-->Warning, email {model.Email} not found (473)");                                
                            };
                        }
                    }
                    else
                    {
                        StatusCode = 472;
                        HttpContext.Response.Headers.Add("Login-Js", "Warning, basic model validation is not valid (472)");
                        LoggerExternal.LoggerEx.Warning("*-->Warning, basic model validation is not valid");
                    }                                        
                }
                else
                {
                    StatusCode = 471;
                    HttpContext.Response.Headers.Add("Login-Js", "Warning, custom model validation is not valid (471)");
                    LoggerExternal.LoggerEx.Warning("*-->Warning, custom model validation is not valid");
                }

                if (authenticateRun)
                {
                    var claimPincipal = new ClaimsPrincipal(await _userService.Authenticate(model.Email, super));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        claimPincipal);
                    //return RedirectToAction("Index", "Home");
                    LoggerExternal.LoggerEx.Information($"*-->User {User.Identity.Name} is authenticated");
                    return Ok();
                }

            }
            catch (Exception ex)
            {
                if (HttpContext.Response.Headers["Login-Js"].ToString() != null)
                {
                    HttpContext.Response.Headers.Remove("Login-Js");
                }
                HttpContext.Response.Headers.Add("Login-Js", $"Main Error {nameof(AppUserLoginJs).ToString()} (470)->{ex.Message}");
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Main Error {nameof(AppUserLoginJs).ToString()}");
            }
            

            
            if (HttpContext.Response.Headers["Login-Js"].ToString() == string.Empty)
                HttpContext.Response.Headers.Add("Login-Js", "Unknow Error!!! (470)");
            LoggerExternal.LoggerEx.Error($"*-->Unkhow Error {nameof(AppUserLoginJs).ToString()}");

            return new StatusCodeResult(StatusCode);
            
           
        }

        [HttpPost]
        public async Task<IActionResult> AppUserLogout()
        {
            HttpContext.SignOutAsync();
            
            //redirect via js
            return RedirectToAction("Index", "Home");
            //return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AppUserLogoutJs()
        {
            HttpContext.SignOutAsync();

            //redirect via js
            //return RedirectToAction("Index", "Home");
            return Ok();
        }

        private bool IsSuperUser(string email, string password="", bool checkName = false)
        {
            string user = email.Substring(0, email.IndexOf("@"));
            bool superUser = email.Substring(0, email.IndexOf("@")) ==
                _configuration["AppSettings:SuperUser"] ? true : false;
            if (checkName && superUser) { return true; }
            if (!superUser) return false;
            bool superPassword =
                WM.GenerateMD5Hash(password, _configuration["AppSettings:PasswordSalt"].ToString()) ==
                _configuration["AppSettings:SuperPassword"].ToString() ? true : false;
            if (!superPassword) return false;
            return true;
        }
        
        [HttpGet]
        public async Task<IActionResult> ProfileCurUser()
        {
            string email = User.Identity.Name.ToString();
            var userDto = await _userService.GetAppUserAsync(email);
            AppUserM appUserM = new AppUserM()
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Description = userDto.Description,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash,
                GroupNumber = userDto.GroupNumber,
                IsEdit = true
            };
            return View(appUserM);
        }
        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangeProfileCurUser([FromBody]Dictionary<string,string> obj)
        {
            try
            {
                var ss = obj;
                string email = obj["email"];
                string name = obj["name"];
                string description = obj["description"];
                AppUserDto userTdo = await _userService.GetAppUserAsync(email);
                bool update = false;
                if (userTdo.Name != name.Trim())
                {
                    update = true;
                    userTdo.Name = name;
                };
                if (userTdo.Description != description.Trim())
                {
                    update = true;
                    userTdo.Description = description;
                };
                if (update)
                {
                    if (! await _userService.UpsertUserAsync(userTdo))
                        return new StatusCodeResult(470);
                }
                else
                {
                    return new StatusCodeResult(475);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Main Error {nameof(ChangeProfileCurUser).ToString()}");
            }
            return new StatusCodeResult(470);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeEmailCurUser([FromBody] Dictionary<string, string> obj)
        {
            try
            {
                var ss = obj;
                string email = obj["email"];
                string emailNew = obj["emailnew"];
                if (emailNew == email) { return new StatusCodeResult(475); }
                AppUserDto userTdo = await _userService.GetAppUserAsync(email);
                userTdo.Email = emailNew;
                if (!await _userService.UpsertUserAsync(userTdo))
                    return new StatusCodeResult(470);
                else
                {
                    HttpContext.SignOutAsync();
                    var claimPincipal = new ClaimsPrincipal(await _userService.Authenticate(emailNew, false));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        claimPincipal);

                }

                return Ok();
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Main Error {nameof(ChangeEmailCurUser).ToString()}");
            }
            return new StatusCodeResult(470);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePswdCurUser([FromBody] Dictionary<string, string> obj)
        {
            try
            {
                var ss = obj;
                string pswd = obj["pswd"];
                string pswdNew = obj["pswdnew"];
                string pswdNewConf = obj["pswdnewconf"];
                string pswdHash = WM.GenerateMD5Hash(pswd, _configuration["AppSettings:PasswordSalt"]);
                string pswdNewHash = WM.GenerateMD5Hash(pswdNew, _configuration["AppSettings:PasswordSalt"]);

                
                AppUserDto userTdo = await _userService.GetAppUserAsync(User.Identity.Name);
                if (userTdo.PasswordHash != pswdHash)
                    return new StatusCodeResult(476);
                if (pswdHash == pswdNewHash) { return new StatusCodeResult(475); }
                userTdo.PasswordHash = pswdNewHash;
                if (!await _userService.UpsertUserAsync(userTdo))
                    return new StatusCodeResult(470);
                else
                {

                    return Ok();
                }

                
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Main Error {nameof(ChangePswdCurUser).ToString()}");
            }
            return new StatusCodeResult(470);
        }
        #endregion

    }

}
