using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetDeviceTagsQuery: IRequest<List<DeviceTag>>
    {
        public int DeviceId { get; set; } = 0;
    }
}
