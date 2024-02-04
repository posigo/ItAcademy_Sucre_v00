using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, List<Device>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetDevicesQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<Device>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
        {
            var devicesDb = await _applicationDbContext.Devices.ToListAsync(cancellationToken);
            
            return devicesDb;
        }
    }
}
