using MediatR;

namespace Sucre_DataAccess.CQS.Commands
{
    public class AddRefreshTokenCommand: IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Ip {  get; set; }
        public int AddMinutes { get; set; }
    }
}
