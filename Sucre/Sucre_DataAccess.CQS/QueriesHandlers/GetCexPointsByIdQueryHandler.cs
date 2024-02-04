using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;
using Sucre_Utility;

namespace Sucre_DataAccess.CQS.QueriesHandlers
{
    public class GetCexPointsByIdQueryHandler : IRequestHandler<GetCexPointsByIdQuery, CexPointsCanalsDto>
    {
        private readonly CanalMapper _chanaleMapper;
        private readonly PointMapper _pointMapper;
        private readonly ApplicationDbContext _applicationDbContext;
        public GetCexPointsByIdQueryHandler(
            ApplicationDbContext applicationDbContext,
            CanalMapper chanaleMapper,
            PointMapper pointMapper)
        {
            _applicationDbContext = applicationDbContext;
            _chanaleMapper = chanaleMapper;
            _pointMapper = pointMapper;
        }

        public async Task<CexPointsCanalsDto> Handle(
            GetCexPointsByIdQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                //Cex cex1 = _applicationDbContext.Cexs
                //.FirstOrDefault(
                //     cex => cex.Id == request.Id);

                Cex cex = _applicationDbContext.Cexs
                    .Where(cex => cex.Id == request.Id)
                    .Include(WC.PointsName)
                    .FirstOrDefault();

                //Cex cex1 = _applicationDbContext.Cexs
                //    .Find(request.Id);

                if (cex == null)
                    return null;
                var result = new CexPointsCanalsDto();
                result.Id = cex.Id;
                result.FullName = WM.GetStringName(new string[]
                {
                    cex.Management,
                    cex.CexName,
                    cex.Area,
                    cex.Device,
                    cex.Location
                });
                List<PointCanalesDto> pointsChanalesDto= new List<PointCanalesDto>();

                //IEnumerable<PointCanalesDto> res = new HashSet<PointCanalesDto>();
                foreach (var point in cex.Points)
                {
                    var pointChanalesDto = new PointCanalesDto();
                    pointChanalesDto.PointDto = _pointMapper.PointToPointDto(point);
                    var chanales = _applicationDbContext.Points
                        .Where(x => x.Id == point.Id)
                        .Include(WC.CanalsName)
                        .Select(item => item.Canals)
                        .FirstOrDefault();
                    
                    var chanalesDto = new List<CanalDto>();
                    
                    foreach (var chanal in chanales)
                        chanalesDto.Add(_chanaleMapper.CanalToCanalDto(chanal));
                    pointChanalesDto.CanalesDto = chanalesDto;
                    pointsChanalesDto.Add(pointChanalesDto);
                }
                result.PointsCanalesDto = pointsChanalesDto;
                return result;
            }catch (Exception ex)
            {
                
            }

            return null;
        }
    }
}
