using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddCexCommandHandler: IRequestHandler<AddCexCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly CexMapper _cexMapper;

        public AddCexCommandHandler(
            ApplicationDbContext applicationDbContext,
            CexMapper cexMapper)
        {
            _applicationDbContext = applicationDbContext;
            _cexMapper = cexMapper;
        }

        public async Task Handle(AddCexCommand request, CancellationToken cancellationToken)
        {
            Cex cex = _cexMapper.CexDtoToCex(request.CexDto);
            var entry = await _applicationDbContext.Cexs.AddAsync(cex,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
        }
    }
}
