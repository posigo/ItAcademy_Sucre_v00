using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_Models;
using Sucre_Services.Interfaces;

namespace Sucre_WebApi.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public TokenController(
            ITokenService tokenService,
            IRoleService roleService,
            IUserService userService)
        {
            _tokenService = tokenService;
            _roleService = roleService;
            _userService = userService;
        }

        [HttpPost]
        [Route("GenerateToken")]
        public async Task<IActionResult> GenerateToken(AppUserLoginM appUserLoginM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new AppUserLoginM { Email = "not empty and example: example@amle.com", Password = "Not empty" });
                //if (!await _userService.IsUserExist(appUserLoginM.Email))
                //    return Unauthorized($"Not found user {appUserLoginM.Email}");
                if (!await _userService.IsPasswordCorrect(
                    password: appUserLoginM.Password,
                    email: appUserLoginM.Email))
                    return Unauthorized($"Not found {appUserLoginM.Email} or password non correct");
                var user = await _userService.GetAppUserAsync(appUserLoginM.Email);
                var roles = await _roleService.GetListRolesByUserIdAsync(user.Id);
                var jwtToken = await _tokenService.GenerateJwtToken(user, roles.ToList());
                var ipValue = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                var rfsToken = await _tokenService.AddRefreshToken(user.Email, ipValue);

                return Ok(new TokenResponseM { AccessToken = jwtToken, RefreshToken = rfsToken });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Unauthorized();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("RefreshToken")]
        public async Task<IActionResult> GenerateToken(Guid refreshTokenId)
        {
            try
            {
                var isRefreshToken = await _tokenService.CheckRefreshToken(refreshTokenId);
                if (isRefreshToken)
                {
                    var userDto = await _userService
                        .GetAppUserByRefreshTokenAsync(refreshTokenId);
                    if (userDto == null)
                        return Unauthorized($"Geting user is null");
                    var roles = await _roleService.GetListRolesByUserIdAsync(userDto.Id);
                    var jwtToken = await _tokenService.GenerateJwtToken(userDto, roles.ToList());
                    var ipValue = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    var rfsToken = await _tokenService.AddRefreshToken(userDto.Email, ipValue);
                    await _tokenService.RemoveRefreshToken(refreshTokenId);
                    return Ok(new TokenResponseM { AccessToken = jwtToken, RefreshToken = rfsToken });
                }
                return Unauthorized($"refresh token with id {refreshTokenId} not valid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("Revoke")]
        public async Task<IActionResult> RevokeToken(RefreshTokenDto refreshTokenDto)
        {
            //todo check if exists to return correct status code
            //todo check that RT for same user as from request
            await _tokenService.RemoveRefreshToken(refreshTokenDto.Id);
            return Ok();
        }

        [HttpGet]
        [Route("GetAllRefreshToken")]
        public async Task<List<RefreshTokenDto>> GetRefreshTokens()
        {
            var result = await _tokenService.GetRefreshTokens();
            return result;
        }

        [HttpGet]
        [Route("GetRefreshTokenById")]
        public async Task<RefreshTokenDto> GetRefreshTokenById(Guid id)
        {
            var result = await _tokenService.GetRefreshTokenById(id);
            return result;
        }

        [HttpGet]
        [Route("GetRefreshTokenByUser")]
        public async Task<List<RefreshTokenDto>> GetRefreshTokenByUser(Guid id)
        {
            //List<RefreshTokenDto>
            //Dictionary<RefreshTokenDto, bool>
            var result = await _tokenService.GetRefreshTokensByUser(id);
            var dict = new Dictionary<RefreshTokenDto, bool>();
            foreach (var item in result)
            {
                var rbool = (item.ExpiringAt.ToUniversalTime() < DateTime.UtcNow &&
                    (DateTime.UtcNow - item.ExpiringAt.ToUniversalTime()).Days == 0)
                    ? true : false;
                dict.Add(item, rbool);  
            }
            return result;
            //return dict;
            //return Ok(dict);
        }

        [HttpGet]
        [Route("ValidateRefreshTokenById")]
        public async Task<IActionResult> GetValidRefreshTokenById(Guid id)
        {
            var usr = User;
           
            var result = await _tokenService.CheckRefreshToken(id);
            return Ok(new { refreshToken = id, valid = result});
        }
    }


}
