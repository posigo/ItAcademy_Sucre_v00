using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddCexCommand: IRequest
    {
        public CexDto CexDto { get; set; }
    }
}
