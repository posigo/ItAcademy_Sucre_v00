using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sucre_Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly TokenMapper _tokenMapper;

        public TokenService(
            IConfiguration configuration,
            IMediator mediator,
            TokenMapper tokenMapper)
        {
            _configuration = configuration;            
            _mediator = mediator;
            _tokenMapper = tokenMapper;
        }

        public async Task<Guid> AddRefreshToken(string reqestEmail, string ipAddress)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByEmailQuery() { Email = reqestEmail });
                Guid refreshTokenId = Guid.NewGuid();
                //var tmpIsAddMin = int.TryParse(
                //    _configuration.GetSection("RefreshJwt:AddMinutes").Value.ToString(),
                //    out var tmpAddMin) ?
                //    tmpAddMin :
                //    30;
                AddRefreshTokenCommand command = new AddRefreshTokenCommand()
                {
                    Id = refreshTokenId,
                    UserId = user.Id,
                    Ip = ipAddress,
                    AddMinutes = int.TryParse(
                            _configuration.GetSection("RefreshJwt:AddMinutes").Value.ToString(),
                            out var addMinutes)
                        ? addMinutes : 30
                };
                await _mediator.Send(command);
                return refreshTokenId;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->TokenService->{nameof(AddRefreshToken)}: {ex.Message}");
                return Guid.Empty;
            }            
        }

        public async Task<bool> CheckRefreshToken(Guid requestRefreshToken)
        {
            var rt = await _mediator.Send(new GetRefreshTokenQuery { Id = requestRefreshToken });
            var rtDto = _tokenMapper.RefreshTokenToRefreshTokenDto(rt);
            //var tmpst = DateTime.UtcNow - rtDto.ExpiringAt.ToUniversalTime();
            if (rtDto != null)
            {
                if (rtDto.IsValid == true)
                    return true;
                if (rtDto.ExpiringAt.ToUniversalTime() < DateTime.UtcNow &&
                    (DateTime.UtcNow - rtDto.ExpiringAt.ToUniversalTime()).Days == 0)
                    return true;
            }
            return false;
        }

        public async Task<string> GenerateJwtToken(AppUserDto userDto, List<AppRoleDto> appRolesDto)
        {
            try
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var secretKey = _configuration.GetSection("Jwt:Secret").Value;
                var signindKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var isLifetime = int.TryParse(_configuration.GetSection("Jwt:Lifetime").Value.ToString(), out var lifetime);

                //var fff = new Claim(JwtRegisteredClaimNames.s)

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, userDto.Email));
                claims.Add(new Claim("GroupId", userDto.GroupNumber.ToString()));
                foreach (var role in appRolesDto)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
                };

                var tokenDescription = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(claims.ToArray()),
                    Issuer = issuer,
                    Audience = audience,
                    Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(lifetime)),
                    NotBefore = DateTime.UtcNow,
                    SigningCredentials = new SigningCredentials(signindKey, SecurityAlgorithms.HmacSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescription);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx
                    .Error($"*->Error, TokenService->{nameof(GenerateJwtToken)}: " +
                    $"Generate token, {ex.Message}");
                return string.Empty;
            }
            
        }

        public async Task<List<RefreshTokenDto>> GetRefreshTokens()
        {
            var refreshTokens = await _mediator.Send(new GetRefreshTokensQuery());
            var refreshTokensDto = refreshTokens
                .Select(rt => _tokenMapper.RefreshTokenToRefreshTokenDto(rt));
            return refreshTokensDto.ToList();
        }

        public async Task<RefreshTokenDto> GetRefreshTokenById(Guid id)
        {
            var refreshToken = (await _mediator
                .Send(new GetRefreshTokensQuery()))
                    .FirstOrDefault(rt => rt.Id.Equals(id));
            var refreshTokenDto = _tokenMapper
                .RefreshTokenToRefreshTokenDto(refreshToken);
            return refreshTokenDto;
        }

        public async Task<List<RefreshTokenDto>> GetRefreshTokensByUser(Guid id)
        {
            var refreshToken = (await _mediator
                .Send(new GetRefreshTokensQuery()))
                    .Where(rt => rt.AppUserId.Equals(id)).ToList();
            var refreshTokensDto = refreshToken
                .Select(rt => _tokenMapper.
                    RefreshTokenToRefreshTokenDto(rt));
            return refreshTokensDto.ToList();
        }

        public async Task RemoveRefreshToken(Guid requestRefreshToken)
        {
            try
            {
                await _mediator.Send(new DeleteRefreshTokenCommand()
                {
                    Id = requestRefreshToken
                });
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx
                    .Error($"*->Error, TokenService->{nameof(RemoveRefreshToken)}: " +
                    $"Remove refresh token, {ex.Message}");
                throw;
            }            

        }
    }
}
