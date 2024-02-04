using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddParameterCommandHandler: IRequestHandler<AddParameterCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ParameterTypeMapper _parameterMapper;

        public AddParameterCommandHandler(
            ApplicationDbContext applicationDbContext,
            ParameterTypeMapper parameterMapper)
        {
            _applicationDbContext = applicationDbContext;
            _parameterMapper = parameterMapper;
        }
        
        public async Task Handle(AddParameterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ParameterType parameter = _parameterMapper.ParameterDtoToParameter(command.parameterDto);
                var entry = await _applicationDbContext.AddAsync(parameter, cancellationToken);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
                var id = entry.Entity.Id;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            //throw new NotImplementedException();
        }
    }
}
