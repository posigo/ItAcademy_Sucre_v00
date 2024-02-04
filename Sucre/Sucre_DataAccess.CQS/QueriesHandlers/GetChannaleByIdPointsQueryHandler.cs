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
    public class GetChannaleByIdPointsQueryHandler : 
        IRequestHandler<GetChannaleByIdPointsQuery, ChannalePointsFullDto>
    {
        private readonly AsPazMapper _asPazMapper;
        private readonly CanalMapper _chanaleMapper;
        private readonly ParameterTypeMapper _parameterTypeMapper;
        private readonly PointMapper _pointMapper;
        private readonly ApplicationDbContext _applicationDbContext;
        public GetChannaleByIdPointsQueryHandler(
            ApplicationDbContext applicationDbContext,
            AsPazMapper asPazMapper,
            CanalMapper chanaleMapper,
            ParameterTypeMapper parameterTypeMapper,
            PointMapper pointMapper)
        {
            _applicationDbContext = applicationDbContext;
            _asPazMapper = asPazMapper;
            _chanaleMapper = chanaleMapper;
            _parameterTypeMapper = parameterTypeMapper;
            _pointMapper = pointMapper;
        }

        public async Task<ChannalePointsFullDto> Handle(
            GetChannaleByIdPointsQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {

                Canal channale = (await _applicationDbContext.Canals
                    .Where(cnl => cnl.Id == request.Id)
                    .Include(WC.ParameterTypeName)
                    .Include(WC.AsPazName)
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy)
                     .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Cex)
                     .ToListAsync(cancellationToken))[0];

                
                if (channale == null)
                    return null;
                ChannalePointsFullDto result = new ChannalePointsFullDto();
                result.CannaleDto = _chanaleMapper.CanalToCanalDto(channale);
                result.ParameterTypeDto = _parameterTypeMapper
                    .ParameterToParameterDto(channale.ParameterType);
                if (channale.AsPazEin && channale.AsPaz != null)
                    result.AsPazDto = _asPazMapper
                        .AsPazToAsPazDto(channale.AsPaz);
                else
                    result.AsPazDto = null;
                if (channale.Points == null)
                {
                    result.PointsTableDto = null;
                }
                else
                {
                    result.PointsTableDto = channale.Points
                    .Select(pnt => new PointTableDto()
                    {
                        PointDto = _pointMapper.PointToPointDto(pnt),
                        EnergyName = pnt.Energy.EnergyName,
                        CexName = WM.GetStringName (new string[]
                        {
                            pnt.Cex.Management,
                            pnt.Cex.CexName,
                            pnt.Cex.Area,
                            pnt.Cex.Device,
                            pnt.Cex.Location
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
