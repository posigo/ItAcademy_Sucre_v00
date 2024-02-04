using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetDeviceTagsQueryHandler : IRequestHandler<GetDeviceTagsQuery, List<DeviceTag>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetDeviceTagsQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<List<DeviceTag>> Handle(GetDeviceTagsQuery request, CancellationToken cancellationToken)
        {
            if (request.DeviceId == 0)
                return await _applicationDbContext.DeviceTags
                    .ToListAsync(cancellationToken);
            else
                return await _applicationDbContext.DeviceTags
                    .Where(item => item.DeviceId == request.DeviceId)
                    .ToListAsync(cancellationToken);
            //var deviceTagsDb = await _applicationDbContext.DeviceTags.ToListAsync(cancellationToken);
            
            //return deviceTagsDb;
        }
    }
}
