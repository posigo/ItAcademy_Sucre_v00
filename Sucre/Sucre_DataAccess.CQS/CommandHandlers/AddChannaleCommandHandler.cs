using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddChannaleCommandHandler: IRequestHandler<AddChannaleCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly CanalMapper _canalMapper;

        public AddChannaleCommandHandler(
            ApplicationDbContext applicationDbContext,
            CanalMapper canalMapper)
        {
            _applicationDbContext = applicationDbContext;
            _canalMapper = canalMapper;
        }

        public async Task Handle(AddChannaleCommand request, CancellationToken cancellationToken)
        {
            Canal canal = _canalMapper.CanalDtoToCanal(request.CanalDto);
            var entry = await _applicationDbContext.Canals.AddAsync(canal,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
        }
    }
}
