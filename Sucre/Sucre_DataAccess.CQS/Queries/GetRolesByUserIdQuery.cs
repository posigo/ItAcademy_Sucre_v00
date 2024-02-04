using MediatR;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.CQS.Queries
{
    public class GetRolesByUserIdQuery : IRequest<List<AppRoleDto>>
    {
        public Guid Id;
    }
}
