using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddDeviceCommand: IRequest
    {
        public Device Device { get; set; }
    }
}
