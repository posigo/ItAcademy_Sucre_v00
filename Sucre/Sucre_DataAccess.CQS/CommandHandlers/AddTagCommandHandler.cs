using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddTagCommandHandler: IRequestHandler<AddTagCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        

        public AddTagCommandHandler(
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(AddTagCommand command, CancellationToken cancellationToken)
        {
            
            var entry = await _applicationDbContext.DeviceTags
                .AddAsync(command.Tag,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
        }
    }
}
