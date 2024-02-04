using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetDeviceByIdQuery: IRequest<Device>
    {
        public int Id { get; set; }
    }
}
