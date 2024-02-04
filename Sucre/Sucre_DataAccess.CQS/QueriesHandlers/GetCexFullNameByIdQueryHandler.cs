using MediatR;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Utility;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetCexFullNameByIdQueryHandler : IRequestHandler<GetCexFullNameByIdQuery, string>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetCexFullNameByIdQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<string> Handle(GetCexFullNameByIdQuery request, CancellationToken cancellationToken)
        {
            Cex cex = await _applicationDbContext.Cexs
                .FindAsync(request.Id, cancellationToken);
            if (cex == null)
                return string.Empty;
            var fullName = WM.GetStringName(new string[]
            {
                cex.Management,
                cex.CexName,
                cex.Area,
                cex.Device,
                cex.Location
            });
            return fullName;
        }
    }
}
