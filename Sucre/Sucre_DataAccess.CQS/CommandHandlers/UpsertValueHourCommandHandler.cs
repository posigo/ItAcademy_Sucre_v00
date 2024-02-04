using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class UpsertValueHourCommandHandler: 
        IRequestHandler<UpsertValueHourCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly CexMapper _cexMapper;

        public UpsertValueHourCommandHandler(
            ApplicationDbContext applicationDbContext//,
            //CexMapper cexMapper
            )
        {
            _applicationDbContext = applicationDbContext;
            //_cexMapper = cexMapper;
        }

        public async Task Handle(UpsertValueHourCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var value = _applicationDbContext.ValuesHour
                    .Where(i => i.Id == request.ValueHour.Id &&
                            i.Date == request.ValueHour.Date &&
                            i.Hour == request.ValueHour.Hour)
                        .Any();
                if (value)
                {
                    request.ValueHour.Changed = true;
                    _applicationDbContext.ValuesHour.Update(request.ValueHour);
                    //var patchsDto = new List<PatchDto>()
                    //    {
                    //        new PatchDto {PropertyName = "Value", PropertyValue = item.Value},
                    //        new PatchDto {PropertyName = "Changed", PropertyValue = true}
                    //    };
                    //var nameValuePairProperties = patchsDto.ToDictionary(key => key.PropertyName,
                    //                                                value => value.PropertyValue);
                    //var dbEntityEntry = _applicationDbContext.ValuesHour.AsQueryable();
                    //var dbEntry = dbEntityEntry.FirstOrDefault(
                    //    i => i.Id == item.Id &&
                    //    i.Date == item.Date &&
                    //    i.Hour == item.Hour);
                    //var entry = _applicationDbContext.Entry(dbEntry);
                    //entry.CurrentValues.SetValues(nameValuePairProperties);
                    //entry.State = EntityState.Modified;
                }
                else
                {
                    var entry = await _applicationDbContext.ValuesHour
                        .AddAsync(request.ValueHour, cancellationToken);
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
