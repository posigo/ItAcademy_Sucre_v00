using MediatR;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetUserByEmailQuery: IRequest<AppUser>
    {
        public string Email { get; set; }
    }
}
