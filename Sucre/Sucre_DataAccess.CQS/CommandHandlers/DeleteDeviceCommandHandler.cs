using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteDeviceCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task Handle(DeleteDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var device = await _applicationDbContext.Devices
                    .FirstOrDefaultAsync(dvc => dvc.Id == command.Id, cancellationToken);
                if (device != null) 
                {
                    _applicationDbContext.Devices.Remove(device);
                    var tags = _applicationDbContext.DeviceTags
                        .Where(tag => tag.DeviceId == command.Id);
                    if (tags != null && tags.Count() > 0)
                    {
                        _applicationDbContext.DeviceTags.RemoveRange(tags);
                    }
                    _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
                
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->Error, Remove device and tags: {ex.Message}");
                //throw;
            }
            
        }
    }
}
