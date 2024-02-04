using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddValuesHourCommandHandler: IRequestHandler<AddValuesHourCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly CexMapper _cexMapper;

        public AddValuesHourCommandHandler(
            ApplicationDbContext applicationDbContext//,
            //CexMapper cexMapper
            )
        {
            _applicationDbContext = applicationDbContext;
            //_cexMapper = cexMapper;
        }

        public async Task Handle(AddValuesHourCommand request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var item in request.ValueHours)
                {
                    var value = _applicationDbContext.ValuesHour
                        .Where(i => i.Id == item.Id &&
                            i.Date == item.Date &&
                            i.Hour == item.Hour)
                        .Any();
                        //.FirstOrDefault()
                        //.Value;
                    if (value)
                    {
                        item.Changed = true;
                        //_applicationDbContext.ValuesHour.Update(item);
                        var patchsDto = new List<PatchDto>()
                        {
                            new PatchDto {PropertyName = "Value", PropertyValue = item.Value},
                            new PatchDto {PropertyName = "Changed", PropertyValue = true}
                        };
                        var nameValuePairProperties = patchsDto.ToDictionary(key => key.PropertyName,
                                                                        value => value.PropertyValue);
                        var dbEntityEntry = _applicationDbContext.ValuesHour.AsQueryable();
                        var dbEntry = dbEntityEntry.FirstOrDefault(
                            i => i.Id == item.Id &&
                            i.Date == item.Date &&
                            i.Hour == item.Hour);
                        var entry = _applicationDbContext.Entry(dbEntry);
                        entry.CurrentValues.SetValues(nameValuePairProperties);
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        var entry = await _applicationDbContext.ValuesHour
                            .AddAsync(item,cancellationToken);
                    }   
                }
                //await _applicationDbContext.SaveChangesAsync(cancellationToken);
                _applicationDbContext.SaveChanges();
            }
            catch ( Exception ex )
            {

            }
             
            
        }
    }
}
