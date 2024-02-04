using MediatR;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;

namespace Sucre_DataAccess.CQS.CommandHandlers
{
    public class AddPointCommandHandler: IRequestHandler<AddPointCommand>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly PointMapper _pointMapper;

        public AddPointCommandHandler(
            ApplicationDbContext applicationDbContext,
            PointMapper pointMapper)
        {
            _applicationDbContext = applicationDbContext;
            _pointMapper = pointMapper;
        }

        public async Task Handle(AddPointCommand request, CancellationToken cancellationToken)
        {
            Point point = _pointMapper.PointDtoToPoint(request.PointDto);
            var entry = await _applicationDbContext.Points
                .AddAsync(point,cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            var id = entry.Entity.Id;
        }
    }
}
