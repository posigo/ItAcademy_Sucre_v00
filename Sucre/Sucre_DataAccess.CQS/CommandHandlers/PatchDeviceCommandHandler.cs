using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class PatchDeviceCommandHandler: 
        IRequestHandler<PatchDeviceCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;        

        public PatchDeviceCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(PatchDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var resultQuery = await _applicationDbContext.Devices                    
                    .FirstOrDefaultAsync(entity => entity.Id.Equals(command.Id), cancellationToken);
                if (resultQuery != null)
                {
                    var nameValuePairProperties = command.PatchDtos
                        .ToDictionary(key => key.PropertyName, value => value.PropertyValue);
                    var dbEntityEntry = _applicationDbContext.Entry(resultQuery);
                    dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
                    dbEntityEntry.State = EntityState.Modified;
                }
                else
                {
                    return;
                }

                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                    
            }
            catch ( Exception ex )
            {
                return;
            }            
            
        }
    }
}
