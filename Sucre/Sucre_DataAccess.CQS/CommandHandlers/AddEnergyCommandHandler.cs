using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_Mappers;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddEnergyCommandHandler: IRequestHandler<AddEnergyCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly EnergyMapper _energyMapper;
        public AddEnergyCommandHandler(
            ApplicationDbContext applicationDbContext, 
            EnergyMapper energyMapper)
        {
            _applicationDbContext = applicationDbContext;
            _energyMapper = energyMapper;
        }
        public async Task Handle(AddEnergyCommand command, CancellationToken cancellationToken)
        {
            var energy = _energyMapper.EnergyDtoToEnergy(command.EnergyDto);
            var entry = await _applicationDbContext.Energies.AddAsync(energy, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
            
            
        }
    }
}
