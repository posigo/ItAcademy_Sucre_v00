using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_WebApi.Models;
using Sucre_Mappers;
using Sucre_Services.Interfaces;
using MediatR;
using Sucre_DataAccess.CQS.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Sucre_WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CexController : ControllerBase
    {
        private readonly ICexService _cexService;
        private readonly IMediator _mediator;
        private readonly CexMapper _cexMapper;

        public CexController(
            ICexService cexService,
            IMediator mediator,
            CexMapper cexMapper)
        {
            _cexService = cexService;
            _mediator = mediator;
            _cexMapper = cexMapper;
        }

        [HttpGet("GetCexWitPointAndChannale/{id}")]
        public async Task<IActionResult> GetCexPointsChanales(int id) 
        {
            var result = await _cexService.GetCexPointsChanalesById(id);
            if (result == null)
                //return NotFound($"Not points and chanales for cex with id ={id} {result}");
                return NotFound(result);
            //return Ok($"Not points and chanales " + result);
            return Ok(result);

        }

        [HttpGet("GetFullNameCex/{id}")]
        public async Task<IActionResult> GetCexFullNameById(int id)
        {
            var fullName = await _mediator.Send(new GetCexFullNameByIdQuery { Id = id });
            if (fullName == string.Empty)
                return NotFound($"Not found object cex with id = {id}");
            var cexFullName = new CexFullNameApiM()
            {
                Id = id,
                FullName = fullName
            };
            return Ok(cexFullName);
        }

        [HttpGet("GetCex/{id}")]
        public async Task<IActionResult> GetCexById(int id)
        {
            var cexDto = await _cexService.GetCexByIdAsync(id);
            if (cexDto == null) { return NotFound($"Not found cex with id={id}");};
            //var cex = _cexMapper.CexDtoToCex(cexDto);
            return Ok(cexDto);
        }

        [HttpGet("GetCexs")]
        public async Task<IActionResult> GetCexs()
        {
            var cexs = await _cexService.GetListCexsAsync();
            if (cexs == null) { return NotFound($"Not found objects cex"); };
            return Ok(cexs);
        }

        [HttpPost("NewCex")]
        public async Task<IActionResult> CreateCex([FromBody] CexApiM cexApiM)
        {
            CexDto cexDto = new CexDto()
            {
                Management = cexApiM.Management,
                CexName = cexApiM.CexName,
                Area = cexApiM.Area,
                Device = cexApiM.Device,
                Location = cexApiM.Location,
                Id = 0
            };
            if (!await _cexService.CreateCexAsync(cexDto))
                return BadRequest($"Not creating object {cexDto}");
            string urlStr = $"{HttpContext.Request.Host.Value}" +
                $"{HttpContext.Request.Path.Value}/" +
                $"{nameof(CreateCex)}/cexname/{cexApiM.ToString()}";
            var mh = HttpContext.GetRouteData();
            var mh1 = HttpContext.GetRouteData().Values;
            var mh2 = HttpContext.GetRouteData().Routers;
            var mh3 = HttpContext.GetRouteData().DataTokens;
            return Created(urlStr,null);
        }
                
        [HttpDelete("RemoveCex/{id}")]
        public async Task<IActionResult> DeleteCex(int id)
        {
            var result = await _cexService.DeleteCexByIdAsync(id);
            if (!result)
                return BadRequest($"Bad request for delete or not found with id={id}");
            return Ok($"Cex remove with id={id} Ok {result}");
        }

        [HttpPatch("UpdateCex/{id}")]
        public async Task<IActionResult> UdateCex(int id, [FromBody] CexDto cexDto)
        {
            if (id == 0) 
                return BadRequest($"Id equal zero");
            if (cexDto == null)
                return BadRequest($"The cexDto object in the request is empty");
            if (cexDto!=null &&
                cexDto.Management == string.Empty &&
                cexDto.CexName == string.Empty &&
                cexDto.Area == string.Empty &&
                cexDto.Device == string.Empty &&
                cexDto.Location == string.Empty)
                return BadRequest($"The fields of the cexDto object in the request are empty");
             
            cexDto.Id = id;
            if (await _cexService.UpsertCexPatchAsync(cexDto))
                return Ok($"The result of changing the object the cex type with Id = {id} is Ok");
            return BadRequest($"The result of changing the object of the cex type with Id = {id} is failed");
        }

    }
}
