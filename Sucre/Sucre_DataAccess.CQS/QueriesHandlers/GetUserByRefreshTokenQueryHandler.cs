using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, AppUser>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetUserByRefreshTokenQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<AppUser> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var refreshToken = await _applicationDbContext.RefreshTokens
                    .FirstOrDefaultAsync(
                        rt => rt.Id.Equals(request.RefreshTokenId),
                        cancellationToken);
                if (refreshToken == null)
                    return null;
                var user = await _applicationDbContext.AppUsers
                    .FirstOrDefaultAsync(
                        usr => usr.Id.Equals(refreshToken.AppUserId),
                        cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->Error, Get user by refresh token: {ex.Message}");
                return null;
            }
        }
    }
}
