using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class DeleteTagCommand: IRequest
    {
        public DeviceTag Tag { get; set; }
    }
}
