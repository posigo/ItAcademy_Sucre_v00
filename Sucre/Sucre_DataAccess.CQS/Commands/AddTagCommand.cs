using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddTagCommand: IRequest
    {
        public DeviceTag Tag { get; set; }
    }
}
