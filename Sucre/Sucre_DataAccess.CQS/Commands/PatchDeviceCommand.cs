using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class PatchDeviceCommand: IRequest
    {
        public int Id { get; set; }
        public List<PatchDto> PatchDtos { get; set; }
    }
}
