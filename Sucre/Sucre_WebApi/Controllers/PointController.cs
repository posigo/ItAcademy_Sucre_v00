using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_DataAccess.CQS.Queries;
using Sucre_Models;
using Sucre_Services;
using Sucre_Services.Interfaces;
using Sucre_WebApi.Models;
using System.Linq;

namespace Sucre_WebApi.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : Controller
    {
        private readonly IPointService _pointService;
        private readonly IMediator _mediator;

        public PointController(
            IPointService pointService,
            IMediator mediator)
        {
            _pointService = pointService;
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("GetAllPoints")]
        public async Task<ActionResult> GetPoints () 
        {
            List<PointDto> pointsDto = new List<PointDto> ();
            var points = await _pointService.GetListPointsAsync();
            if (points == null)
                return NotFound($"Not found list point metering");
            pointsDto = points.ToList();
            return Ok(pointsDto);
        }

        [HttpGet()]
        [Route("GetAllPointsWithFullDescription")]
        public async Task<ActionResult> GetPointsFullName()
        {
            //List<PointApiM> pointsDto = new List<PointApiM>();
            var pointsTableDto = await _pointService.GetPointsFullAsync();
            if (pointsTableDto == null) return NotFound("Not found points");
            var pointsApiM = pointsTableDto
                .Select(point => new PointApiM()
                {
                    Id = point.PointDto.Id,
                    Name = point.PointDto.Name,
                    Description = point.PointDto.Description,
                    EnergyName = point.EnergyName,
                    CexName = point.CexName,
                    ServiceStaff = point.PointDto?.ServiceStaff
                }).ToList();

            return Ok(pointsApiM);
        }

        [HttpGet("GetPoint/{id}")]
        [HttpGet]
        //[Route("GetPoint/{Id}")]
        public async Task<IActionResult> GetPointById(int id)
        {
            var pointDto = await _pointService.GetPointByIdAsync(id);
            if (pointDto == null) return NotFound($"Not found point with Id={id}");
            return Ok(pointDto);
        }

        [HttpGet("GetPointWithFullDescription/{id}")]
        //[Route("GetPointWithFullDescription")]
        public async Task<IActionResult> GetPointFullById(int id)
        {
            var pointDto = await _pointService.GetPointsFullByIdAsync(id);
            if (pointDto == null) return NotFound($"Not found point with Id={id}");
            return Ok(new PointApiM()
            {
                Id=pointDto.PointDto.Id,
                Name = pointDto.PointDto.Name,
                Description = pointDto.PointDto.Description,
                EnergyName = pointDto.EnergyName,
                CexName = pointDto.CexName,
                ServiceStaff=pointDto.PointDto?.ServiceStaff
            });
        }

        [HttpGet("GetPointWithAssignedChannales/{id}/{fullName}")]
        //[Route("GetPointWithAssignedChannales")]
        public async Task<IActionResult> GetPointByIdAndChannales(int id, bool fullName = false)
        {
            object pointChannales = null ;
            if (!fullName)
            {
                //PointCanalesDto
                pointChannales = await _pointService.GetPointCanalesAsync(id);
                if (pointChannales == null) return NotFound($"Not found point with Id={id}");
                return Ok(pointChannales);
            }
            else
            {
                //var
                pointChannales = await _pointService.GetPointCanalesFullAsync(id);
            }
            if (pointChannales == null) return NotFound($"Not found point with Id={id}");
            return Ok(pointChannales);
           
        }

        [HttpPost]
        [Route("NewPoint")]
        public async Task<IActionResult> CreatePoint([FromBody] PointCreateApiM pointCreateM)
        {
            string urlStr = string.Empty;
            if (pointCreateM.EnergyId <= 0)
                ModelState.AddModelError("EnergyId", "No correct field value");
            if (pointCreateM.CexId <= 0)
                ModelState.AddModelError("CexId", "No correct field value");
            if (!ModelState.IsValid)
            {
                string strErrors = string.Empty;                
                foreach (var mdSt in ModelState)
                {
                    strErrors += $"{mdSt.Key.ToString()}: {mdSt.Value.Errors[0].ErrorMessage}";
                    strErrors += System.Environment.NewLine;
                };
                return BadRequest($"{strErrors}");
            }
            PointDto pointDto = new PointDto()
            {
                Id = 0,
                Name = pointCreateM.Name,
                Description = pointCreateM.Description,
                EnergyId = pointCreateM.EnergyId,
                CexId = pointCreateM.CexId,
                ServiceStaff = pointCreateM.ServiceStaff
            };
            if (! await _pointService.CreatePointAsync(pointDto))
            {
                return BadRequest($"Not creating object {pointDto}");
            }
            urlStr = $"{HttpContext.Request.Host.Value}" +
                $"{HttpContext.Request.Path.Value}/" +
                $"{nameof(CreatePoint)}/{pointCreateM.Name}";
            return Created(urlStr, null);
        }

        [HttpDelete("RemovePoint/{id}")]
        //[Route("RemovePoint")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            var result = await _pointService.DeletePointByIdAsync(id);
            if (!result)
                return BadRequest($"Bad request for delete or not found with id={id}");
            return Ok($"Point remove with id={id} Ok {result}");
        }

        [HttpPatch("UpdatePoint/{id}")]
        //[Route("UpdatePoint")]
        public async Task<IActionResult> UpdateCex(int id, [FromBody] PointDto pointDto)
        {
            if (id == 0)
                return BadRequest($"Id equal zero");
            if (pointDto == null)
                return BadRequest($"The pointDto object in the request is empty");
            if (pointDto != null &&
                (pointDto.Name.Trim() == string.Empty ||
                pointDto.EnergyId <= 0 ||
                pointDto.CexId <= 0))
            {
                return BadRequest($"The fields of the pointDto object in the request are empty");
            }
            if (await _pointService.UpsertPointPatchAsync(pointDto))
                return Ok($"The result of changing the object the point with Id = {id} is Ok");
            return BadRequest($"The result of changing the object of the point with Id = {id} is failed");
        }
    }

    
}
