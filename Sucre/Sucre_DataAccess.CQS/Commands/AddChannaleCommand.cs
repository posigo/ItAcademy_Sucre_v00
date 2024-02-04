using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddChannaleCommand: IRequest
    {
        public CanalDto CanalDto { get; set; }
    }
}
