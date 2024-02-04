using Sucre_Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sucre_Services.Interfaces
{
    public interface IMeasurementService
    {
        Task<MeasurementPointDto> GetHourValueByPoint(int id, string? strDate, int? hour);
        Task<MeasurementPointHDto> GetDayValueByPoint(int id, string? strDateB, string? strDateE);
        /// <summary>
        /// Отчёт по часам
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="queryDate"></param>
        /// <param name="queryHour"></param>
        /// <returns></returns>
        Task<ValuesHourByIdPointDto> GetValuesHourByPointId(int pointId, string? queryDate, int? queryHour);
        /// <summary>
        /// Отчёт по суткам
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="queryDate"></param>
        /// <param name="queryDateE"></param>
        /// <returns></returns>
        Task<ValuesDayByIdPointDto> GetValuesDayByPointId(int pointId, string? queryDate, string? queryDateE);
    }
}
