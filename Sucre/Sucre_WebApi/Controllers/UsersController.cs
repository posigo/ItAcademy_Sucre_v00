using Microsoft.AspNetCore.Mvc;
using Sucre_Models;
using Sucre_Services.Interfaces;

namespace Sucre_WebApi.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public UsersController(
            IRoleService roleService,
            IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpPost]
        [Route("AppUserLogin")]
        public async Task<ActionResult> LoginUserById([FromBody] AppUserLoginM appUserLoginM)
        {
            if (ModelState.IsValid)
            {
                if (!await _userService.IsUserExist(appUserLoginM.Email))
                    return NotFound($"Not found user {appUserLoginM.Email}");
                if (!await _userService.IsPasswordCorrect(appUserLoginM.Password))
                    return Unauthorized($"password non correct");
                var user = await _userService.GetAppUserAsync(appUserLoginM.Email);
                var roles = await _roleService.GetListRolesByUserIdAsync(user.Id);
                return Ok( new { userId= user.Id, userv = user, rolesv = roles });
            }
            return Unauthorized($"{appUserLoginM.Email}");
        }

    }
}
