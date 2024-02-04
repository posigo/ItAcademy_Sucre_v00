using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Utility;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetPointsFullQueryHandler : IRequestHandler<GetPointsFullQuery, List<Point>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetPointsFullQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<Point>> Handle(GetPointsFullQuery request, CancellationToken cancellationToken)
        {
            var pointsDb = await _applicationDbContext.Points
                .Include(WC.EnergyName)
                .Include(WC.CexName)
                .ToListAsync(cancellationToken);

            return pointsDb;
        }
    }
}
