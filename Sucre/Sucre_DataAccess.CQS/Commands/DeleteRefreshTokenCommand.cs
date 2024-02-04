using MediatR;

namespace Sucre_DataAccess.CQS.Commands
{
    public class DeleteRefreshTokenCommand: IRequest
    {
        public Guid Id { get; set; }
    }
}
