using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddPointCommand: IRequest
    {
        public PointDto PointDto { get; set; }
    }
}
