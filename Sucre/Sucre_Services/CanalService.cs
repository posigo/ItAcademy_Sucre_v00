using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Sucre_Core;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;
using System.Globalization;
using System.Linq.Expressions;

namespace Sucre_Services
{
    public class CanalService : ICanalService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly AsPazMapper _asPazMapper;
        private readonly CanalMapper _canalMapper;
        private readonly ParameterTypeMapper _parameterTypeMapper;
        private readonly PointMapper _pointMapper;

        public CanalService(IConfiguration configuration,
            IMediator mediator,
            ISucreUnitOfWork sucreUnitOfWork,
            AsPazMapper asPazMapper,
            CanalMapper canalMapper,
            ParameterTypeMapper parameterTypeMapper,
            PointMapper pointMapper)
        {
            _configuration = configuration;
            _mediator = mediator;
            _sucreUnitOfWork = sucreUnitOfWork;
            _asPazMapper = asPazMapper;
            _canalMapper = canalMapper;
            _parameterTypeMapper = parameterTypeMapper;
            _pointMapper = pointMapper;
        }

        public async Task<bool> CreateChannaleAsync(CanalDto canalDto)
        {
            try
            {
                var addChannaleCommand = new AddChannaleCommand()
                {
                    CanalDto = canalDto,
                };
                _mediator.Send(addChannaleCommand);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService-> {nameof(CreateChannaleAsync)}");
            }
            return false;
        }

        public async Task<CanalDto> GetCannaleByIdAsync(int Id)
        {
            try
            {
                var cannaleDb = await _sucreUnitOfWork.repoSucreCanal.FindAsync(Id);
                return (cannaleDb != null)
                    ? _canalMapper.CanalToCanalDto(cannaleDb)
                    : null;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService-> {nameof(GetCannaleByIdAsync)}");
            }
            return null;
        }

        public async Task<IEnumerable<CanalDto>> GetListCannalesAsync()
        {
            try
            {
                var cannalesDb = await _sucreUnitOfWork
                    .repoSucreCanal
                    .GetAllAsync();
                IEnumerable<CanalDto> cannalesDto = cannalesDb
                    .Select(cannale => _canalMapper.CanalToCanalDto(cannale));
                return cannalesDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, CexService->  {nameof(GetListCannalesAsync)}");
            }
            return null;
        }

        public async Task<CannaleFullDto> GetCannaleByIdFullAsync(int Id)
        {
            try
            {
                CannaleFullDto cannaleFullDto = new CannaleFullDto();
                cannaleFullDto = (await GelListCannalesFullAsync(Id)).ToList()[0];


                return cannaleFullDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService-> {nameof(GetCannaleByIdFullAsync)}");
            }
            return null;
        }

        /// <summary>
        /// Get a channale and assigned metering points
        /// </summary>
        /// <param name="Id">Id channale</param>       
        /// <returns></returns>
        public async Task<ChannalePointsDto> GetChannalePointesAsync(int Id)
        {
            try
            {
                var channaleDb = await _sucreUnitOfWork.repoSucreCanal
                    .FirstOrDefaultAsync(
                    filter: channale => channale.Id == Id,
                    includeProperties: $"{WC.PointsName}");

                if (channaleDb == null) { return null; }

                ChannalePointsDto channalePointsDto = new ChannalePointsDto();
                channalePointsDto.ChannaleDto = _canalMapper.CanalToCanalDto(channaleDb);
                if (channaleDb.Points.Count == 0)
                {
                    channalePointsDto.PointsDto = new HashSet<PointDto>();
                }
                else
                {
                    channalePointsDto.PointsDto = channaleDb.Points
                        .Select(point => _pointMapper.PointToPointDto(point));
                }

                return channalePointsDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, "!!!->Error when getting a channale with points");
                return null;
            }
        }

        public async Task<ChannalePointsFullDto> GetChannalePointsFullAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    LoggerExternal.LoggerEx.Warning($"*-->Warnning, PointService-> {nameof(GetChannalePointsFullAsync)}: Id zero");
                    return null;
                }
                GetChannaleByIdPointsQuery query = new GetChannaleByIdPointsQuery()
                {
                    Id = id
                };
                ChannalePointsFullDto result = await _mediator.Send(query);
                if (result == null)
                {
                    LoggerExternal.LoggerEx.Information($"*-->PointService-> {nameof(GetChannalePointsFullAsync)}: Point with Id={id} in empty");
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, PointService-> {nameof(GetChannalePointsFullAsync)}");
                return null;
            }

        }

        public async Task<IEnumerable<CannaleFullDto>> GelListCannalesFullAsync(int? Id)
        {
            try
            {
                Expression<Func<Canal, bool>> epFilter = null;
                if (Id != null && Id.Value != 0)
                {
                    epFilter = item => item.Id == Id.Value;
                }
                var cannalesDb = await _sucreUnitOfWork.repoSucreCanal
                    .GetAllAsync(
                        filter: epFilter,
                        includeProperties: $"{WC.ParameterTypeName},{WC.AsPazName}");


                var cannalesFullDto =
                    new HashSet<CannaleFullDto>();
                foreach (var cannale in cannalesDb)
                {
                    CannaleFullDto cannaleFullDto = new CannaleFullDto();
                    cannaleFullDto.CannaleDto = _canalMapper
                        .CanalToCanalDto(cannale);
                    cannaleFullDto.ParameterTypeDto = _parameterTypeMapper
                        .ParameterToParameterDto(cannale.ParameterType);
                    if (cannale.AsPaz != null)
                    {
                        cannaleFullDto.AsPazDto = _asPazMapper
                            .AsPazToAsPazDto(cannale.AsPaz);
                    }
                    else
                    {
                        cannaleFullDto.AsPazDto = null;
                    }
                    cannalesFullDto.Add(cannaleFullDto);
                    //sdf.Add(cannaleFullDto);
                }

                return cannalesFullDto;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService->{nameof(GelListCannalesFullAsync)}");
            }
            return null;
        }

        public List<CanalShortNameDto> GetListCanalesForId(
            List<int> listIds = null,
            bool tEqual = false,
            bool paramName = false)
        {
            Expression<Func<Canal, bool>> epFilter = null;
            if (listIds.Count != 0)
            {
                bool begId = true;
                foreach (var idCanale in listIds)
                {
                    if (begId)
                    {
                        if (!tEqual)
                            epFilter = item => item.Id != idCanale;
                        else
                            epFilter = item => item.Id == idCanale;
                        begId = false;
                    }
                    else
                    {
                        if (!tEqual)
                            epFilter = epFilter.And(item => item.Id != idCanale);
                        else
                            epFilter = epFilter.And(item => item.Id == idCanale);
                    }
                }
            };
            string includeValue = (paramName) ? WC.ParameterTypeName : null;
            var cannalesDb = _sucreUnitOfWork.repoSucreCanal.GetAll(
                filter: epFilter,
                includeProperties: ((paramName) ? WC.ParameterTypeName : null),
                isTracking: false);
            List<CanalShortNameDto> cannalesShortNameDto = cannalesDb
                .Select(canalDb => new CanalShortNameDto
                {
                    Id = canalDb.Id,
                    Name = canalDb.Name,
                    ParameterTypeId = canalDb.ParameterTypeId,
                    ParameterTypeName = ((paramName)
                        ? $"{canalDb.ParameterType.Mnemo},{canalDb.ParameterType.UnitMeas}"
                        : string.Empty)
                }).ToList();
            return cannalesShortNameDto;
        }

        public async Task<bool> UpsertCanalAsync(CanalDto canalDto)
        {
            try
            {
                if (canalDto == null) { return false; }
                Canal canal = _canalMapper.CanalDtoToCanal(canalDto);
                if (canalDto.Id == null || canalDto.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucreCanal.AddAsync(canal);
                }
                else
                {
                    //energy.Id = energyDto.Id;

                    await _sucreUnitOfWork.repoSucreCanal.Patch(canal.Id, new List<PatchDto>()
                        {
                            new() {PropertyName = nameof(canal.Name),PropertyValue = canal.Name},
                            new() {PropertyName = nameof(canal.Description), PropertyValue = canal.Description},
                            new() {PropertyName = nameof(canal.ParameterTypeId),PropertyValue = canal.ParameterTypeId},
                            new() {PropertyName = nameof(canal.Reader),PropertyValue = canal.Reader},
                            new() {PropertyName = nameof(canal.SourceType),PropertyValue = canal.SourceType},
                            new() {PropertyName = nameof(canal.AsPazEin),PropertyValue = canal.AsPazEin},
                            new() {PropertyName = nameof(canal.Hour),PropertyValue = canal.Hour},
                            new() {PropertyName = nameof(canal.Day),PropertyValue = canal.Day},
                            new() {PropertyName = nameof(canal.Month),PropertyValue = canal.Month}
                        });

                    //await _sucreUnitOfWork.repoSucrePoint .UpdateAsync(point);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, PointService->   {nameof(UpsertCanalAsync)}");
            }

            return false;
        }

        /// <summary>
        /// Remove/Add menering point in channale
        /// </summary>
        /// <param name="Id">Id channale</param>
        /// <param name="IdPoint">Id point</param>
        /// <param name="upsert">false-Remove/true-Add</param>
        /// <returns>result (boolean)</returns>
        public async Task<bool> UpsertPointToCanal(int Id, int IdPoint, bool upsert = false)
        {
            try
            {
                Canal canalDb = await _sucreUnitOfWork
                    .repoSucreCanal
                    .FirstOrDefaultAsync(
                        filter: item => item.Id == Id,
                        includeProperties: WC.PointsName);

                Point point;
                if (!upsert)
                {
                    point = canalDb.Points.FirstOrDefault(item => item.Id == IdPoint);
                    canalDb.Points.Remove(point);
                }
                else
                {
                    Point addPointDb = await _sucreUnitOfWork
                        .repoSucrePoint
                        .FirstOrDefaultAsync(
                            filter: item => item.Id == IdPoint,
                            includeProperties: $"{WC.EnergyName},{WC.CexName}");
                    canalDb.Points.Add(addPointDb);
                }

                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService->{nameof(UpsertPointToCanal)}");
            }
            return false;

        }

        public async Task<bool> DeleteChannaleAsync(CanalDto channaleDto)
        {
            try
            {
                if (channaleDto == null) return false;
                Canal channale = new Canal();
                channale = _canalMapper.CanalDtoToCanal(channaleDto);
                await _sucreUnitOfWork.repoSucreCanal.RemoveAsync(channale);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService->{nameof(DeleteChannaleAsync)}");
            }
            return false;
        }

        public async Task<bool> DeleteChannaleByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucreCanal.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, CanalService-> {nameof(DeleteChannaleByIdAsync)}");
            }
            return false;
        }

        static int z = 0;
        public async Task Z_HangFire(int type, params string[] values)
        {
            switch (type)
            {
                case 1:
                    if (values != null)
                    {
                        Console.WriteLine($"{values[0]}, thank you contacting us.");
                    }
                    break;
                case 2:
                    if (values != null)
                    {
                        Console.WriteLine($"Session for client {values[0]} has been closed");
                    }
                    break;
                case 3:
                    Console.WriteLine($"Read parameter {z++}");
                    break;
                case 4:
                    break;
            }
        }


        #region ReadApi
        //удалить?
        private async Task<decimal> ReadFromApi((int, string) tuple)
        {
            decimal result = decimal.Parse("-999999,9");

            using (var client = new HttpClient())
            {
                string strUri = string.Empty;
                if (tuple.Item2 == null)
                    strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                else
                    strUri = tuple.Item2;
                client.BaseAddress = new Uri(strUri);
                var sRequest = $"{strUri}?id={tuple.Item1}";
                var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                result = decimal.Parse(sResponce.Replace(".", ","));
            };
            return result;
        }
        //удалить?
        public async Task<bool> ReadValueChannaleFromDevice(int id, string baseUri)
        {
            //List<Canal> channales = new List<Canal>();
            List<ValueHour> values = new List<ValueHour>();
            try
            {
                var channalesI = await _sucreUnitOfWork.repoSucreCanal
                    .FirstOrDefaultAsync(
                        filter: cnl => cnl.Id == id);

                var channaleA = await _sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Id == id)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy)
                     .FirstOrDefaultAsync();

                if (channaleA != null &&
                    channaleA.Reader &&
                    channaleA.SourceType == 0)
                {
                    (int, string) tuple = (id, null);
                    ValueHour valueHour = new ValueHour()
                    {
                        Id = id,
                        Date = DateTime.Now.Date,
                        Hour = DateTime.Now.Hour,
                        Value = await ReadFromApi(tuple),
                        Changed = false
                    };
                    UpsertValueHourCommand command = new UpsertValueHourCommand()
                    {
                        ValueHour = valueHour,
                    };
                    await _mediator.Send(command);
                    return true;
                }
                else
                    return false;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }
        //удалить?
        public async Task<bool> ReadValuesChannalesFromDevice(string baseUri)
        {
            //List<Canal> channales = new List<Canal>();
            List<ValueHour> values = new List<ValueHour>();
            try
            {
                var channalesI = ((_sucreUnitOfWork.repoSucreCanal
                .GetAllAsync(
                    filter: cnl => cnl.Reader == true && cnl.SourceType == 0,
                    includeProperties: $"{WC.ParameterTypeName},ValueHours"))
                    .GetAwaiter().GetResult())
                    .ToList();
                var channalesA = (_sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Reader == true && cnl.SourceType == 0 &&
                        cnl.Hour == true)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy)).ToList();
                using (var client = new HttpClient())
                {
                    string strUri = string.Empty;
                    if (baseUri == null)
                        strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                    else
                        strUri = baseUri;
                    client.BaseAddress = new Uri(strUri);
                    foreach (var canal in channalesA)
                    {
                        var sRequest = $"{strUri}?id={canal.Id}";
                        var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                        var value = decimal.Parse(sResponce.Replace(".", ","));
                        ValueHour valueHour = new ValueHour()
                        {
                            Id = canal.Id,
                            Date = DateTime.Now.Date,
                            Hour = DateTime.Now.Hour,
                            Value = value,
                            Changed = false
                        };
                        values.Add(valueHour);

                       
                    }
                    AddValuesHourCommand command = new AddValuesHourCommand()
                    {
                        ValueHours = values
                    };
                    _mediator.Send(command);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }

        public async Task<bool> ReadValuesHour()
        {
            //List<Canal> channales = new List<Canal>();
            List<ValueHour> values = new List<ValueHour>();
            try
            {
                List<int> channalesId = (_sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Reader == true && cnl.SourceType == 0 &&
                        cnl.Hour == true)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy))
                    .Select(cnl => cnl.Id)
                    .ToList();
                GetListReadIdQuery query = new GetListReadIdQuery()
                {
                    ReadId = channalesId
                    //ReadId = hh
                };

                List<ChannaleRead> channalesRead = await _mediator.Send(query);

                using (var client = new HttpClient())
                {
                    string strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                    client.BaseAddress = new Uri(strUri);
                    foreach (var canal in channalesRead)
                    {
                        var sRequest = $"{canal.UrlAPi}{canal.Query}";
                        var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                        var value = decimal.Parse(sResponce.Replace(".", ","));
                        ValueHour valueHour = new ValueHour()
                        {
                            Id = canal.ChannaleId,
                            Date = DateTime.Now.Date,
                            Hour = DateTime.Now.Hour,
                            Value = value,
                            Changed = false
                        };
                        values.Add(valueHour);
                    }
                    AddValuesHourCommand command = new AddValuesHourCommand()
                    {
                        ValueHours = values
                    };
                    await _mediator.Send(command);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }
        public async Task<bool> ReadValuesHourMan(int id, DateTime date, int? hour)
        {
            //List<Canal> channales = new List<Canal>();
            List<ValueHour> values = new List<ValueHour>();
            ValueHour valueHour;
            try
            {
                List<int> channalesId = await _sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Reader == true && cnl.SourceType == 0 &&
                        cnl.Hour == true && cnl.Id == id)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy)
                    .Select(cnl => cnl.Id)
                    .ToListAsync();
                GetListReadIdQuery query = new GetListReadIdQuery()
                {
                    ReadId = channalesId
                    //ReadId = hh
                };

                List<ChannaleRead> channalesRead = await _mediator.Send(query);//.GetAwaiter().GetResult();

                using (var client = new HttpClient())
                {
                    string strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                    client.BaseAddress = new Uri(strUri);
                    if (hour != null)
                    {
                        valueHour = new ValueHour()
                        {
                            Id = id,
                            Date = date.Date,
                            Hour = hour.Value,
                            Changed = false
                        };
                        var sRequest = $"{channalesRead[0].UrlAPi}{channalesRead[0].Query}";
                        var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                        valueHour.Value = decimal.Parse(sResponce.Replace(".", ","));
                        values.Add(valueHour);
                    }
                    else
                    {
                        for (int i = 0; i < 24; i++) 
                        {
                            valueHour = new ValueHour()
                            {
                                Id = id,
                                Date = date.Date,
                                Hour = i,
                                Changed = false
                            };
                            var sRequest = $"{channalesRead[0].UrlAPi}{channalesRead[0].Query}";
                            var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                            valueHour.Value = decimal.Parse(sResponce.Replace(".", ","));
                            values.Add(valueHour);
                        }
                    }
                    
                    AddValuesHourCommand command = new AddValuesHourCommand()
                    {
                        ValueHours = values
                    };
                    await _mediator.Send(command);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }

        public async Task<bool> ReadValuesDay()
        {
            //List<Canal> channales = new List<Canal>();
            List<ValueDay> values = new List<ValueDay>();
            try
            {
                List<int> channalesId = (_sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Reader == true && cnl.SourceType == 0 &&
                        cnl.Day == true)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy))
                    .Select(cnl => cnl.Id)
                    .ToList();
                GetListReadIdQuery query = new GetListReadIdQuery()
                {
                    ReadId = channalesId
                    //ReadId = hh
                };

                List<ChannaleRead> channalesRead = await _mediator.Send(query);

                using (var client = new HttpClient())
                {
                    string strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                    client.BaseAddress = new Uri(strUri);
                    foreach (var canal in channalesRead)
                    {
                        var sRequest = $"{canal.UrlAPi}{canal.Query}";
                        var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                        var value = decimal.Parse(sResponce.Replace(".", ","));
                        ValueDay valueDay = new ValueDay()
                        {
                            Id = canal.ChannaleId,
                            Date = DateTime.Now.Date,
                            Value = value,
                            Changed = false
                        };
                        values.Add(valueDay);
                    }
                    AddValuesDayCommand command = new AddValuesDayCommand()
                    {
                        ValueDays = values
                    };
                    await _mediator.Send(command);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }
        public async Task<bool> ReadValuesDayMan(int id, DateTime dateb, DateTime? datee)
        {
            //if(date == null && month == null) return false;

            //List<Canal> channales = new List<Canal>();
            List<ValueDay> values = new List<ValueDay>();
            ValueDay valueDay;
            try
            {
                List<int> channalesId = (_sucreUnitOfWork.repoSucreCanal.GetAsQueryable()
                    .Where(cnl => cnl.Reader == true && cnl.SourceType == 0 &&
                        cnl.Hour == true && cnl.Id == id)
                    .Include(WC.ParameterTypeName)
                    .Include("ValueHours")
                    .Include(cnl => cnl.Points)
                        .ThenInclude(pnt => pnt.Energy))
                    .Select(cnl => cnl.Id)
                    .ToList();
                GetListReadIdQuery query = new GetListReadIdQuery()
                {
                    ReadId = channalesId
                    //ReadId = hh
                };

                List<ChannaleRead> channalesRead = await _mediator.Send(query);

                using (var client = new HttpClient())
                {
                    string strUri = "https://localhost:7294/api/PhysicalParameterRandom/GetRandomValue";
                    client.BaseAddress = new Uri(strUri);
                    if (datee == null)
                    {
                        valueDay = new ValueDay()
                        {
                            Id = id,                            
                            Date = dateb.Date,                            
                            Changed = false
                        };
                        var sRequest = $"{channalesRead[0].UrlAPi}{channalesRead[0].Query}";
                        var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                        valueDay.Value = decimal.Parse(sResponce.Replace(".", ","));
                        values.Add(valueDay);
                    }
                    else
                    {
                        for (var i = dateb; i < datee.Value; i=i.AddDays(1))
                        {
                            valueDay = new ValueDay()
                            {
                                Id = id,
                                Date = i.Date,
                                
                                Changed = false
                            };
                            var sRequest = $"{channalesRead[0].UrlAPi}{channalesRead[0].Query}";
                            var sResponce = (await client.GetStringAsync(sRequest)).ToString();
                            valueDay.Value = decimal.Parse(sResponce.Replace(".", ","));
                            values.Add(valueDay);
                        }
                    }

                    AddValuesDayCommand command = new AddValuesDayCommand()
                    {
                        ValueDays = values
                    };
                    await _mediator.Send(command);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false;
        }

      

        #endregion

    }
}
