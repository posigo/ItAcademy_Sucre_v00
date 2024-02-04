using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetEnergyByIdQueryHandler : IRequestHandler<GetEnergyByIdQuery, Energy>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetEnergyByIdQueryHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Energy> Handle(GetEnergyByIdQuery request, CancellationToken cancellationToken)
        {
            var energy = await _applicationDbContext.Energies
                .FirstOrDefaultAsync(
                    energy => energy.Id.Equals(request.Id), 
                    cancellationToken);
            return energy;
        }
    }
}
