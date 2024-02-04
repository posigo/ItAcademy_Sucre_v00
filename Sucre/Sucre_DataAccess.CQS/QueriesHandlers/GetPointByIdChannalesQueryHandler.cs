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
    public class GetPointByIdChannalesQueryHandler: 
        IRequestHandler<GetPointByIdChannalesQuery, PointChannalesFullDto>
    {
        private readonly CanalMapper _chanaleMapper;
        private readonly PointMapper _pointMapper;
        private readonly ApplicationDbContext _applicationDbContext;
        public GetPointByIdChannalesQueryHandler(
            ApplicationDbContext applicationDbContext,
            CanalMapper chanaleMapper,
            PointMapper pointMapper)
        {
            _applicationDbContext = applicationDbContext;
            _chanaleMapper = chanaleMapper;
            _pointMapper = pointMapper;
        }

        public async Task<PointChannalesFullDto> Handle(
            GetPointByIdChannalesQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {

                Point point = (await _applicationDbContext.Points
                    .Where(pnt => pnt.Id == request.Id)
                    .Include(WC.EnergyName)
                    .Include(WC.CexName)
                    .Include(pnt=>pnt.Canals)
                        .ThenInclude(cnl => cnl.ParameterType)
                    .Include(pnt => pnt.Canals)
                        .ThenInclude(cnl => cnl.AsPaz)
                    .ToListAsync(cancellationToken))[0];
                if (point == null)
                    return null;
                PointChannalesFullDto result = new PointChannalesFullDto();
                result.PointTableDto = new PointTableDto()
                {
                    PointDto = _pointMapper.PointToPointDto(point),
                    EnergyName = point.Energy.EnergyName,
                    CexName = WM.GetStringName(new string[]
                    {
                        point.Cex.Management,
                        point.Cex.CexName,
                        point.Cex.Area,
                        point.Cex.Device,
                        point.Cex.Location
                    })
                };
                if (point.Canals == null)
                {
                    result.ChannalesShortDto = null;
                }
                else
                {
                    result.ChannalesShortDto = point.Canals
                    .Select(cnl => new CanalShortNameDto()
                    {
                        Id = cnl.Id,
                        Name = cnl.Name,
                        ParameterTypeId = cnl.ParameterTypeId,
                        ParameterTypeName = WM.GetStringName(new string[]
                        {
                            cnl.ParameterType.Name,
                            cnl.ParameterType.Mnemo,
                            cnl.ParameterType.UnitMeas
                        })
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }
}
