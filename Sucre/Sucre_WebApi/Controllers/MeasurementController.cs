using Microsoft.AspNetCore.Mvc;
using Sucre_Services.Interfaces;

namespace Sucre_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(
            IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetHourValues(int id, string? vdate, int? hour)
        {            
            var result = await _measurementService.GetHourValueByPoint(id, vdate, hour);
            //await _measurementService.GetHourValueByPoint(1, null);
            //await _measurementService.GetHourValueByPoint(1, 17);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetHourReport(int id, string? vdate, int? hour)
        {
            var result = await _measurementService.GetValuesHourByPointId(id, vdate, hour);
            //await _measurementService.GetHourValueByPoint(1, null);
            //await _measurementService.GetHourValueByPoint(1, 17);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDayValues(int id, string? dateb, string? datee)
        {
            var result = await _measurementService.GetDayValueByPoint(id, dateb, datee);
            //await _measurementService.GetHourValueByPoint(1, null);
            //await _measurementService.GetHourValueByPoint(1, 17);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDayReport(int id, string? dateb, string? datee)
        {
            var result = await _measurementService.GetValuesDayByPointId(id, dateb, datee);
            //await _measurementService.GetHourValueByPoint(1, null);
            //await _measurementService.GetHourValueByPoint(1, 17);
            return Ok(result);
        }
    }
}
