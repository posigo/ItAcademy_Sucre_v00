using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class UpdateTagCommandHandler : 
        IRequestHandler<UpdateTagCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;        

        public UpdateTagCommandHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Handle(UpdateTagCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var resultQuery = await _applicationDbContext.DeviceTags                    
                    .FirstOrDefaultAsync(
                        entity => entity.ChannaleId.Equals(command.ChannaleId)
                        , cancellationToken);
                if (resultQuery != null)
                {
                    DeviceTag deviceTag = new DeviceTag()
                    {
                        Id = resultQuery.Id,
                        DeviceId = resultQuery.DeviceId,
                        ChannaleId = resultQuery.ChannaleId,
                        Enviroment = command.Environment,
                        ParameterCode = command.ParameterCode
                    };
                    resultQuery.Enviroment = deviceTag.Enviroment;
                    resultQuery.ParameterCode = deviceTag.ParameterCode;
                    //_applicationDbContext.DeviceTags.Update(deviceTag);
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
