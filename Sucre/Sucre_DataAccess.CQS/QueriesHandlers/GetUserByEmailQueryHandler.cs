using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, AppUser>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetUserByEmailQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<AppUser> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.AppUsers.
                FirstOrDefaultAsync(
                    usr => usr.Email.Equals(request.Email),
                    cancellationToken);
            return user;
        }
    }
}
