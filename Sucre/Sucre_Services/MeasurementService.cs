using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Linq.Expressions;

namespace Sucre_Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMediator _mediator;
        private readonly AsPazMapper _asPazMapper;
        private readonly CanalMapper _channaleMapper;
        private readonly ParameterTypeMapper _parameterMapper;
        private readonly PointMapper _pointMapper;
        private readonly ValueMapper _valueMapper;

        public MeasurementService(
            ApplicationDbContext applicationDbContext,
            IMediator mediator,
            AsPazMapper asPazMapper,
            CanalMapper channaleMapper,
            ParameterTypeMapper parameterMapper,
            PointMapper pointMapper,
            ValueMapper valueMapper)
        {
            _applicationDbContext = applicationDbContext;            
            _mediator = mediator;
            _asPazMapper = asPazMapper;
            _channaleMapper = channaleMapper;
            _parameterMapper = parameterMapper;
            _pointMapper = pointMapper;
            _valueMapper = valueMapper;
        }

        public async Task<MeasurementPointDto> GetHourValueByPoint(int id, string? strDate, int? hour)
        {
            var meas = new MeasurementPointDto();
            List<ValueHour> valuesHour;
            try
            {
                Point point = await _applicationDbContext.Points
                    .Where(pnt => pnt.Id == id)
                    .Include(WC.EnergyName)
                    .Include(WC.CexName)
                    .Include(WC.CanalsName)
                    .Include(pnt => pnt.Canals)
                        .ThenInclude(cnl => cnl.ParameterType)
                    .Include(pnt => pnt.Canals)
                        .ThenInclude(cnl => cnl.AsPaz)
                    .FirstOrDefaultAsync(); ;
                // = new List<ValueHour>();
                //1. Get full info point and channales with parameter

                var chanalesId = point.Canals.Select(cnl => cnl.Id).ToList();
                Expression<Func<ValueHour, bool>> epFilter = null;
                epFilter = vh => chanalesId.Contains(vh.Id);
                if (strDate == null && hour == null)
                    epFilter = vh => chanalesId.Contains(vh.Id);
                if (strDate == null && hour != null)
                    epFilter = epFilter.And(vh => vh.Hour == hour.Value);
                if (strDate != null && hour == null)
                    epFilter = epFilter.And(vh => vh.Date.Date == Convert.ToDateTime(strDate).Date);
                if (strDate != null && hour != null)
                {
                    epFilter = epFilter.And(vh => vh.Hour == hour.Value);
                    epFilter = epFilter.And(vh => vh.Date.Date == Convert.ToDateTime(strDate).Date);
                }
                valuesHour = await _applicationDbContext.ValuesHour
                    .Where(epFilter)
                    .ToListAsync();
                
                meas.PointTableDto.PointDto = _pointMapper.PointToPointDto(point);
                meas.PointTableDto.EnergyName = point.Energy.EnergyName;
                meas.PointTableDto.CexName = WM.GetStringName(new string[]
                {
                    point.Cex.Management,
                    point.Cex.CexName,
                    point.Cex.Area,
                    point.Cex.Device,
                    point.Cex.Location
                });
                var channales = point.Canals;
                List<CannaleFullDto> channalesFullDto = new List<CannaleFullDto>();
                List<ValueHourDto> valuesHourDto = new List<ValueHourDto>();
                foreach (var c in channales)
                {
                    var cd = new CannaleFullDto()
                    {
                        CannaleDto = _channaleMapper.CanalToCanalDto(c),
                        ParameterTypeDto = _parameterMapper.ParameterToParameterDto(c.ParameterType),
                        AsPazDto = c.AsPaz == null ? null : _asPazMapper.AsPazToAsPazDto(c.AsPaz)
                    };
                    channalesFullDto.Add(cd);
                }
                meas.ChannalesFullDto = channalesFullDto;
                meas.ValuesHourDto  = valuesHour.Select(vh => _valueMapper.ValueHourToValueHourDto(vh)).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }

            
            return meas;



            //2 Get value hour by channales




           // throw new NotImplementedException();
        }

        public async Task<MeasurementPointHDto> GetDayValueByPoint(int id, string? strDateB, string? strDateE)
        {
            var meas = new MeasurementPointHDto();
            List<ValueDay> valuesDay;
            try
            {
                Point point = await _applicationDbContext.Points
                    .Where(pnt => pnt.Id == id)
                    .Include(WC.EnergyName)
                    .Include(WC.CexName)
                    .Include(WC.CanalsName)
                    .Include(pnt => pnt.Canals)
                        .ThenInclude(cnl => cnl.ParameterType)
                    .Include(pnt => pnt.Canals)
                        .ThenInclude(cnl => cnl.AsPaz)
                    .FirstOrDefaultAsync(); ;
                
                var chanalesId = point.Canals.Select(cnl => cnl.Id).ToList();
                Expression<Func<ValueDay, bool>> epFilter = null;
                epFilter = vh => chanalesId.Contains(vh.Id);
                if (strDateB == null && strDateE == null)
                    epFilter = vh => chanalesId.Contains(vh.Id);
                if (strDateB == null && strDateE != null)
                    epFilter = epFilter.And(vh => vh.Date.Date == Convert.ToDateTime(strDateE).Date);
                if (strDateB != null && strDateE == null)
                    epFilter = epFilter.And(vh => vh.Date.Date == Convert.ToDateTime(strDateB).Date);
                if (strDateB != null && strDateE != null)
                {
                    epFilter = epFilter.And(vh => vh.Date.Date >= Convert.ToDateTime(strDateB).Date);
                    epFilter = epFilter.And(vh => vh.Date.Date <= Convert.ToDateTime(strDateE).Date);
                }
                valuesDay = await _applicationDbContext.ValuesDay
                    .Where(epFilter)
                    .ToListAsync();

                meas.PointTableDto.PointDto = _pointMapper.PointToPointDto(point);
                meas.PointTableDto.EnergyName = point.Energy.EnergyName;
                meas.PointTableDto.CexName = WM.GetStringName(new string[]
                {
                    point.Cex.Management,
                    point.Cex.CexName,
                    point.Cex.Area,
                    point.Cex.Device,
                    point.Cex.Location
                });
                var channales = point.Canals;
                List<CannaleFullDto> channalesFullDto = new List<CannaleFullDto>();
                List<ValueDayDto> valuesHourDto = new List<ValueDayDto>();
                foreach (var c in channales)
                {
                    var cd = new CannaleFullDto()
                    {
                        CannaleDto = _channaleMapper.CanalToCanalDto(c),
                        ParameterTypeDto = _parameterMapper.ParameterToParameterDto(c.ParameterType),
                        AsPazDto = c.AsPaz == null ? null : _asPazMapper.AsPazToAsPazDto(c.AsPaz)
                    };
                    channalesFullDto.Add(cd);
                }
                meas.ChannalesFullDto = channalesFullDto;
                meas.ValuesDayDto = valuesDay.Select(vh => _valueMapper.ValueDayToValueDayDto(vh)).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }


            return meas;

        }

        public async Task<ValuesHourByIdPointDto> GetValuesHourByPointId(int pointId, string? queryDate, int? queryHour)
        {
            //данные по часам
            var valueh = await GetHourValueByPoint(pointId, queryDate, queryHour);
            ValuesHourByIdPointDto valuesHourByIdPoint = new ValuesHourByIdPointDto();
            valuesHourByIdPoint.ValuesHourPoint = valueh;
            //заголовок отчета
            var heading = $"Отчёт: {Environment.NewLine} {valueh.PointTableDto.PointDto.Name} ";
            if (queryDate != null)
            {
                var tmpHead = $"{Environment.NewLine} за {queryDate.Replace("/", ".")}";
                heading += tmpHead;
            }
            valuesHourByIdPoint.Heading = heading;
            //каналы точки учёта
            if (valueh.ChannalesFullDto.Count == 0)
            {
                valuesHourByIdPoint.Columns = 0;
                valuesHourByIdPoint.ColumnsName = null;
            }
            else
            {
                valuesHourByIdPoint.Columns = valueh.ChannalesFullDto.Count + 1;
                (int, List<string>) tupleNameCol;
                var strNameColl = new List<(int, List<string>)>();
                tupleNameCol = (0, new List<string>() { "Час" });
                strNameColl.Add(tupleNameCol);
                foreach (var cnl in valueh.ChannalesFullDto)
                {
                    tupleNameCol = (cnl.CannaleDto.Id, new List<string>()
                    {
                        $"{cnl.CannaleDto.Name}",
                        $"{cnl.ParameterTypeDto.Mnemo}, {cnl.ParameterTypeDto.UnitMeas}"
                    });
                    strNameColl.Add(tupleNameCol);
                }




                //var strNameColl = new List<List<string>>();

                //strNameColl.Add(new List<string>() { "Час" });
                //foreach (var c in valueh.ChannalesFullDto)
                //    strNameColl.Add (new List<string>()
                //    {
                //        $"{c.CannaleDto.Name}",
                //        $"{c.ParameterTypeDto.Mnemo}, {c.ParameterTypeDto.UnitMeas}"
                //    });
                valuesHourByIdPoint.ColumnsName = strNameColl.ToArray();


            }
            //данные
            valuesHourByIdPoint.Rows = valueh.ValuesHourDto.Count;

            if (valueh.ValuesHourDto.Count > 0)
            {
                var tableDic = new Dictionary<int, decimal[]>();
                var decimalMas = new List<decimal>();
                //if (queryHour != null)
                //{
                for (var hour = ((queryHour == null) ? 0 : queryHour.Value);
                    hour <= ((queryHour == null) ? 23 : queryHour.Value);
                    hour++)
                {
                    foreach (var cnl in valueh.ChannalesFullDto)
                    {
                        var valueHour = valueh.ValuesHourDto
                            .Where(value => value.Id == cnl.CannaleDto.Id && value.Hour == hour)
                            .Select(v => v.Value)
                            .FirstOrDefault();
                        if (valueHour != null)
                            decimalMas.Add(valueHour);
                        else
                            decimalMas.Add(0);
                    }
                    tableDic.Add(hour, decimalMas.ToArray());
                    decimalMas.Clear();
                }
                //}
                valuesHourByIdPoint.TableDict = tableDic;
            }
            else
            {
                valuesHourByIdPoint.TableDict = null;
            }


            return valuesHourByIdPoint;
        }

        public async Task<ValuesDayByIdPointDto> GetValuesDayByPointId(
            int pointId,
            string? queryDate,
            string? queryDateE)
        {
            //получить данные с дд1 по дд2
            var valued = await GetDayValueByPoint(pointId, queryDate, queryDateE);
            //отчёт для отображения
            ValuesDayByIdPointDto valuesDayByIdPoint = new ValuesDayByIdPointDto();
            valuesDayByIdPoint.ValuesDayPoint = valued;
            //заголовок
            var heading = $"Отчёт: {Environment.NewLine} {valued.PointTableDto.PointDto.Name} ";
            if (queryDate != null)
            {
                if (queryDateE != null)
                {
                    var tmpHead = $"{Environment.NewLine} c {queryDate.Replace("/", ".")} по {queryDateE.Replace("/", ".")}";
                    heading += tmpHead;
                }
                else
                {
                    var tmpHead = $"{Environment.NewLine} за {queryDate.Replace("/", ".")}";
                    heading += tmpHead;
                }

            }
            valuesDayByIdPoint.Heading = heading;
            //список каналов
            if (valued.ChannalesFullDto.Count == 0)
            {
                valuesDayByIdPoint.Columns = 0;
                valuesDayByIdPoint.ColumnsName = null;
            }
            else
            {
                valuesDayByIdPoint.Columns = valued.ChannalesFullDto.Count + 1;
                //названия колонок
                (int, List<string>) tupleNameCol;
                var strNameColl = new List<(int, List<string>)>();
                //первая колонка
                tupleNameCol = (0, new List<string>() { "Дата" });
                strNameColl.Add(tupleNameCol);
                foreach (var cnl in valued.ChannalesFullDto)
                {
                    tupleNameCol = (cnl.CannaleDto.Id, new List<string>()
                    {
                        $"{cnl.CannaleDto.Name}",
                        $"{cnl.ParameterTypeDto.Mnemo}, {cnl.ParameterTypeDto.UnitMeas}"
                    });
                    strNameColl.Add(tupleNameCol);
                }

                valuesDayByIdPoint.ColumnsName = strNameColl.ToArray();


            }
            valuesDayByIdPoint.Rows = valued.ValuesDayDto.Count;
            //значения строк по колонкам
            if (valued.ValuesDayDto.Count > 0)
            {
                var tableDic = new Dictionary<DateTime, decimal[]>();
                var decimalMas = new List<decimal>();
                var ddateE = queryDateE == null ?
                    DateTime.Parse(queryDate) :
                    DateTime.Parse(queryDateE);
                for (var ddate = DateTime.Parse(queryDate);
                    ddate <= ddateE; ddate = ddate.AddDays(1))
                {
                    foreach (var cnl in valued.ChannalesFullDto)
                    {
                        var valueDay = valued.ValuesDayDto
                            .Where(value => value.Id == cnl.CannaleDto.Id && value.Date == ddate)
                            .Select(v => v.Value)
                            .FirstOrDefault();
                        if (valueDay != null)
                            decimalMas.Add(valueDay);
                        else
                            decimalMas.Add(0);
                    }
                    tableDic.Add(ddate.Date, decimalMas.ToArray());
                    decimalMas.Clear();
                }
                //}
                valuesDayByIdPoint.TableDict = tableDic;
            }
            else
            {
                valuesDayByIdPoint.TableDict = null;
            }

            return valuesDayByIdPoint;
            //return View(valuesDayByIdPointM);
        }
    }
}
