using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddParameterCommand: IRequest
    {
        public ParameterTypeDto parameterDto { get; set; }
    }
}
