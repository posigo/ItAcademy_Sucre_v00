using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddDeviceCommandHandler: IRequestHandler<AddDeviceCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        

        public AddDeviceCommandHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(AddDeviceCommand command, CancellationToken cancellationToken)
        {
            
            var entry = await _applicationDbContext.Devices
                .AddAsync(command.Device,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
        }
    }
}
