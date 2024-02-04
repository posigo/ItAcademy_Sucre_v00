using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Utility;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetPointsFullByIdQueryHandler : IRequestHandler<GetPointsFullByIdQuery, Point>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetPointsFullByIdQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Point> Handle(GetPointsFullByIdQuery request, CancellationToken cancellationToken)
        {
            //var pointDb = await _applicationDbContext.Points
            //    .FindAsync(request.Id, cancellationToken);

            var pointDb = (await _applicationDbContext.Points
                .Where(pnt => pnt.Id == request.Id)
                .Include(WC.EnergyName)
                .Include(WC.CexName)
                .AsNoTracking()
                .ToListAsync(cancellationToken))[0];
            
             return pointDb;
        }
    }
}
