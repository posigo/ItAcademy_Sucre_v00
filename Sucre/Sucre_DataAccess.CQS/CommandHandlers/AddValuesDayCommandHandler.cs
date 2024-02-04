using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddValuesDayCommandHandler: IRequestHandler<AddValuesDayCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AddValuesDayCommandHandler(
            ApplicationDbContext applicationDbContext
            )
        {
            _applicationDbContext = applicationDbContext;
            //_cexMapper = cexMapper;
        }

        public async Task Handle(AddValuesDayCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var item in request.ValueDays)
                {
                    var value = _applicationDbContext.ValuesDay
                        .Where(i => i.Id == item.Id &&
                            i.Date == item.Date)
                        .Any();
                    if (value)
                    {
                        item.Changed = true;                        
                        var patchsDto = new List<PatchDto>()
                        {
                            new PatchDto {PropertyName = "Value", PropertyValue = item.Value},
                            new PatchDto {PropertyName = "Changed", PropertyValue = true}
                        };
                        var nameValuePairProperties = patchsDto.ToDictionary(key => key.PropertyName,
                                                                        value => value.PropertyValue);
                        var dbEntityEntry = _applicationDbContext.ValuesDay.AsQueryable();
                        var dbEntry = dbEntityEntry.FirstOrDefault(
                            i => i.Id == item.Id &&
                            i.Date == item.Date);
                        var entry = _applicationDbContext.Entry(dbEntry);
                        entry.CurrentValues.SetValues(nameValuePairProperties);
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        var entry = await _applicationDbContext.ValuesDay
                            .AddAsync(item,cancellationToken);
                    }   
                }
                await _applicationDbContext.SaveChangesAsync(cancellationToken);                
            }
            catch ( Exception ex )
            {

            }
             
            
        }
    }
}
