using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteTagCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _applicationDbContext.DeviceTags
                     .FirstOrDefaultAsync(dvc => dvc.DeviceId == command.Tag.DeviceId &&
                        dvc.ChannaleId == command.Tag.ChannaleId, cancellationToken);
                if (tag != null) 
                {
                    _applicationDbContext.DeviceTags.Remove(tag);
                    
                    _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->Error, Remove link tags: {ex.Message}");
                //throw;
            }
            
        }
    }
}
