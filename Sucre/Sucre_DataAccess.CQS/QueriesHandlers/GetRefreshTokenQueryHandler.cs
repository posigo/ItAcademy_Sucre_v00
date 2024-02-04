using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, RefreshToken>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetRefreshTokenQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<RefreshToken> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await _applicationDbContext.RefreshTokens
                .FirstOrDefaultAsync(
                    rt => rt.Id.Equals(request.Id),
                    cancellationToken);
            return refreshToken;
        }
    }
}
