using MediatR;

namespace Sucre_DataAccess.CQS.Commands
{
    public class UpdateTagCommand : IRequest
    {
        public int DeviceId { get; set; }
        public int ChannaleId { get; set; }
        public int Environment { get; set; }
        public int ParameterCode {  get; set; }
    }
}
