using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetRefreshTokenQuery: IRequest<RefreshToken>
    {
        public Guid Id { get; set; }
    }
}
