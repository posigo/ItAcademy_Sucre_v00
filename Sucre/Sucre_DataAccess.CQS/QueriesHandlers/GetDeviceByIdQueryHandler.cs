using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Device>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetDeviceByIdQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Device> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _applicationDbContext.Devices
                .Where(dvc => dvc.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            
            return device;
        }
    }
}
