using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetUserByRefreshTokenQuery: IRequest<AppUser>
    {
        public Guid RefreshTokenId { get; set; }
    }
}
