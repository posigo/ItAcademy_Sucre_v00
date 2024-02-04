using LinqKit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Linq.Expressions;
using Point = Sucre_DataAccess.Entities.Point;

namespace Sucre_Services
{
    public class PointService: IPointService
    {
        private readonly IConfiguration _configuration;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly CanalMapper _canalMapper;
        private readonly PointMapper _pointMapper;        
        private readonly IMediator _mediator;

        public PointService(IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            IMediator mediator,
            CanalMapper canalMapper,
            PointMapper pointMapper)
        {
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _canalMapper = canalMapper;
            _pointMapper = pointMapper;
            _mediator = mediator;
        }

        public async Task<bool> AddCanalToPoint(int Id, int IdCannale)
        {
            try
            {
                Point pointDb = await _sucreUnitOfWork
                    .repoSucrePoint
                    .FirstOrDefaultAsync(
                        filter: item => item.Id == Id,
                        includeProperties: WC.CanalsName);
                Canal addCannaleDb = await _sucreUnitOfWork
                    .repoSucreCanal
                    .FirstOrDefaultAsync(
                        filter: item => item.Id == IdCannale,
                        includeProperties: WC.ParameterTypeName);
                pointDb.Canals.Add(addCannaleDb);
                //_sucreUnitOfWork.Commit();
                await _sucreUnitOfWork.CommitAsync();


                return true;
            }
            catch (Exception ex)
            {
                //LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService->{nameof(DeleteCanalFromPoint)}");
            }
            return false;

        }

        public async Task<bool> CreatePointAsync(PointDto pointDto)
        {
            try
            {
                var addPointCommand = new AddPointCommand()
                {
                    PointDto = pointDto
                };
                _mediator.Send(addPointCommand);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        /// <summary>
        /// Remove/Add cannale in menering point
        /// </summary>
        /// <param name="Id">Id point</param>
        /// <param name="IdCannale">Id cannale</param>
        /// <param name="upsert">false-Remove/true-Add</param>
        /// <returns>result (boolean)</returns>
        public async Task<bool> UpsertCanalToPoint(int Id, int IdCannale, bool upsert = false)
        {
            try
            {
                Sucre_DataAccess.Entities.Point pointDb = await _sucreUnitOfWork
                   .repoSucrePoint
                    .FirstOrDefaultAsync(
                        filter: item => item.Id == Id,
                        includeProperties: WC.CanalsName);
                Canal canal;
                if (!upsert)
                {
                    canal = pointDb.Canals.FirstOrDefault(item => item.Id == IdCannale);
                    pointDb.Canals.Remove(canal);
                }
                else
                {
                    Canal addCannaleDb = await _sucreUnitOfWork
                    .repoSucreCanal
                    .FirstOrDefaultAsync(
                        filter: item => item.Id == IdCannale,
                        includeProperties: WC.ParameterTypeName);
                    pointDb.Canals.Add(addCannaleDb);
                }                
                
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService->{nameof(UpsertCanalToPoint)}");
            }
            return false;
            
        }

        public async Task<bool> DeletePointAsync(PointDto pointDto)
        {
            try
            {
                if (pointDto == null) return false;                
                //Point point = _pointMapper.PointDtoToPoint(pointDto);
                //await _sucreUnitOfWork.repoSucrePoint.RemoveAsync(point);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService->{nameof(DeletePointAsync)}");
            }
            return false;
        }

        public async Task<bool> DeletePointByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucrePoint.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService-> {nameof(DeletePointByIdAsync)}");
            }
            return false;
        }

        public List<PointShortNameDto> GetListPointsForId(
            List<int> listIds = null,
            bool tEqual = false,
            bool paramName = true)
        {
            Expression<Func<Point, bool>> epFilter = null;
            if (listIds.Count != 0)
            {
                bool begId = true;
                foreach (var idPoint in listIds)
                {
                    if (begId)
                    {
                        if (!tEqual)
                            epFilter = item => item.Id != idPoint;
                        else
                            epFilter = item => item.Id == idPoint;
                        begId = false;
                    }
                    else
                    {
                        if (!tEqual)
                            epFilter = epFilter.And(item => item.Id != idPoint);
                        else
                            epFilter = epFilter.And(item => item.Id == idPoint);
                    }
                }
            };
            string includeValue = (paramName) ? $"{WC.EnergyName},{WC.CexName}" : null;
            var pointsDb = _sucreUnitOfWork.repoSucrePoint.GetAll(
                filter: epFilter,
                includeProperties: includeValue,
                isTracking: false);
            List<PointShortNameDto> pointsShortNameDto = pointsDb
                .Select(pointDb => new PointShortNameDto
                {
                    Id = pointDb.Id,
                    Name = pointDb.Name,
                    EnergyId = pointDb.EnergyId,
                    EnergyName = pointDb.Energy.EnergyName,
                    CexId = pointDb.CexId,
                    CexName = ((paramName)
                        ? WM.GetStringName(new string[]
                        {
                            pointDb.Cex.Management,
                            pointDb.Cex.CexName,
                            pointDb.Cex.Area,
                            pointDb.Cex.Device,
                            pointDb.Cex.Location
                        })
                        : string.Empty
                    )
                }).ToList();
            return pointsShortNameDto;
        }

        public async Task<PointDto> GetPointByIdAsync(int Id)
        {
            try
            {
                var pointDb = await _sucreUnitOfWork.repoSucrePoint.FindAsync(Id);                
                return (pointDb != null)
                    ? _pointMapper.PointToPointDto(pointDb)
                    : null;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService-> {nameof(GetPointByIdAsync)}");
            }
            return null;
        }

        public async Task<PointTableDto> GetPointsFullByIdAsync(int id)
        {
            var result = new PointTableDto();
            try
            {
                var pointDb = await _mediator.Send(new GetPointsFullByIdQuery() { Id = id });
                if (pointDb == null) return null;
                result.PointDto = _pointMapper.PointToPointDto(pointDb);
                result.EnergyName = pointDb.Energy.EnergyName;
                result.CexName = WM.GetStringName(new string[]
                {
                    pointDb.Cex.Management,
                    pointDb.Cex.CexName,
                    pointDb.Cex.Area,
                    pointDb.Cex.Device,
                    pointDb.Cex.Location
                });
               
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->  {nameof(GetPointsFullByIdAsync)}");
                result = null;
            }
            return result;
        }

        public async Task<IEnumerable<PointDto>> GetListPointsAsync()
        {
            try
            {
                var pointsDb = await _sucreUnitOfWork
                    .repoSucrePoint
                    .GetAllAsync();
                IEnumerable<PointDto> pointsDto = pointsDb
                    .Select(point => _pointMapper.PointToPointDto(point));                    
                return pointsDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->  {nameof(GetListPointsAsync)}");
            }
            return null;
        }
        
        public async Task<IEnumerable<PointTableDto>> GetListPointsByStrAsync()
        {
            try
            {
                var pointsDb = await _sucreUnitOfWork
                    .repoSucrePoint
                    .GetAllAsync(includeProperties: $"{WC.EnergyName},{WC.CexName}");
                IEnumerable<PointTableDto> pointsTableDto = pointsDb
                    .Select(point => new PointTableDto
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
                    });
                return pointsTableDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->  {nameof(GetListPointsByStrAsync)}");
            }
            return null;
        }

        public async Task<List<PointTableDto>> GetPointsFullAsync()
        {
            var result = new List<PointTableDto>();
            try
            {                
                var pointsDb = await _mediator.Send(new GetPointsFullQuery());
                foreach (var point in pointsDb)
                {
                    result.Add(new PointTableDto()
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
                    });
                };
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->  {nameof(GetPointsFullAsync)}");
                result = null;
            }
            return result;
        }

        public async Task<PointTableDto> GetPointByIdStrAsync(int Id)
        {
            try
            {            
                var point = await _sucreUnitOfWork.repoSucrePoint
                    .FirstOrDefaultAsync(filter: point => point.Id == Id,
                    includeProperties: $"{WC.EnergyName},{WC.CexName}",
                    isTracking: false);
                PointTableDto pointTableDto = new PointTableDto
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
                return pointTableDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService-> {nameof(GetPointByIdStrAsync)}");
            }
            return null;
        }

        /// <summary>
        /// Get a metering point and assigned canales. Get canales not assigned point
        /// </summary>
        /// <param name="id">id metering point</param>       
        /// <returns></returns>
        public async Task<PointCanalesDto> GetPointCanalesAsync(int id)
        {
            try
            {
                var pointDb = await _sucreUnitOfWork.repoSucrePoint
                    .FirstOrDefaultAsync(
                    filter: item => item.Id == id,
                    includeProperties: WC.CanalsName);

                //var appUserDb = _sucreUnitOfWork.repoSucreAppUser.FirstOrDefault(
                //    filter: user => user.Id == id,
                //    includeProperties: $"{WC.AppRolesName}",
                //    isTracking: false);

                if (pointDb == null) { return null; }

                PointCanalesDto pointCanalesDto = new PointCanalesDto();
                pointCanalesDto.PointDto = _pointMapper.PointToPointDto(pointDb);
                if (pointDb.Canals.Count == 0)
                {
                    pointCanalesDto.CanalesDto = new HashSet<CanalDto>();
                }
                else
                {
                    pointCanalesDto.CanalesDto = pointDb.Canals
                        .Select(canal => _canalMapper.CanalToCanalDto(canal));
                }
                return pointCanalesDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error when getting a point with canales");
                return null;
            }
        }

        public async Task<PointChannalesFullDto> GetPointCanalesFullAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    LoggerExternal.LoggerEx.Warning($"*-->Warnning, PointService-> {nameof(GetPointCanalesFullAsync)}: Id zero");   
                    return null; 
                }
                GetPointByIdChannalesQuery query = new GetPointByIdChannalesQuery()
                {
                    Id = id
                };
                PointChannalesFullDto result = await _mediator.Send(query);
                if (result == null) 
                {
                    LoggerExternal.LoggerEx.Information($"*-->PointService-> {nameof(GetPointCanalesFullAsync)}: Point with Id={id} in empty");
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService-> {nameof(GetPointCanalesFullAsync)}");
                return null;
            }
            
        }

        public async Task<bool> UpsertPointPatchAsync(PointDto pointDto)
        {
            try
            {
                if (pointDto == null) { return false; }
                Sucre_DataAccess.Entities.Point point = new Sucre_DataAccess.Entities.Point();
                point = _pointMapper.PointDtoToPoint(pointDto);
                await _sucreUnitOfWork.repoSucrePoint.Patch(point.Id, new List<PatchDto>()
                {
                    new() {PropertyName = nameof(point.Name),PropertyValue = point.Name},
                    new() {PropertyName = nameof(point.Description),PropertyValue = point.Description},
                    new() {PropertyName = nameof(point.EnergyId),PropertyValue = point.EnergyId},
                    new() {PropertyName = nameof(point.CexId),PropertyValue = point.CexId},
                    new() {PropertyName = nameof(point.ServiceStaff),PropertyValue = point.ServiceStaff},

                });

                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->   {nameof(UpsertPointPatchAsync)}");
            }

            return false;
        }

        public async Task<bool> UpsertPointAsync(PointDto pointDto, bool patch = false)
        {
            try
            {
                if (pointDto == null) { return false; }
                Sucre_DataAccess.Entities.Point point = new Sucre_DataAccess.Entities.Point();
                point = _pointMapper.PointDtoToPoint(pointDto);                
                if (pointDto.Id == null || pointDto.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucrePoint.AddAsync(point);
                }
                else
                {
                    //energy.Id = energyDto.Id;

                    if (patch)
                        await _sucreUnitOfWork.repoSucrePoint.Patch(point.Id, new List<PatchDto>()
                        {
                            new() {PropertyName = nameof(point.Name),PropertyValue = point.Name},
                            new() {PropertyName = nameof(point.Description),PropertyValue = point.Description},
                            new() {PropertyName = nameof(point.EnergyId),PropertyValue = point.EnergyId},
                            new() {PropertyName = nameof(point.CexId),PropertyValue = point.CexId},
                            new() {PropertyName = nameof(point.ServiceStaff),PropertyValue = point.ServiceStaff},

                        });
                    else
                        _sucreUnitOfWork.repoSucrePoint.Update(point);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->   {nameof(UpsertPointAsync)}");
            }
            
            return false;
        }

    }
}
