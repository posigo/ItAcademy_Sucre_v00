using MediatR;

namespace Sucre_DataAccess.CQS.Commands
{
    public class DeleteDeviceCommand: IRequest
    {
        public int Id { get; set; }
    }
}
