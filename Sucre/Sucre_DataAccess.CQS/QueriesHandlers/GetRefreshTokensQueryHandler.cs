using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetRefreshTokensQueryHandler : IRequestHandler<GetRefreshTokensQuery, List<RefreshToken>>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public GetRefreshTokensQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<RefreshToken>> Handle(GetRefreshTokensQuery request, CancellationToken cancellationToken)
        {
            var refreshTokens = await _applicationDbContext.RefreshTokens
                .ToListAsync(cancellationToken);
            return refreshTokens;
        }
    }
}
