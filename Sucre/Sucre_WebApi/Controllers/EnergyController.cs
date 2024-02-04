using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_Services.Interfaces;

namespace Sucre_WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyController : ControllerBase
    {
        private readonly IEnergyService _energyService;
        private readonly EnergyMapper _energyMapper;
       

        public EnergyController(
            IEnergyService energyService,
            EnergyMapper energyMapper) 
        { 
            _energyService = energyService;
            _energyMapper = energyMapper;
        }

        [HttpGet("GetEnergy/{id}")]
        public async Task<IActionResult> GetEnergyById(int id)
        {
            //var energy = await _energyService.GetEnergyTypeByIdAsync(id);
            var energyDto = await _energyService.GetEnergyByIdAsync(id);
            if (energyDto == null) { return NotFound($"Not found energy with id={id}"); }
            return Ok(energyDto);
        }

        [HttpGet()]
        public async Task<IActionResult> GetEnergies()
        {
            var energies = await _energyService.GetListEnergyTypesAsync();
            if (energies == null) { return NotFound("Not found"); }
            return Ok(energies);
        }

        [HttpPost("NewEnergy")]
        public async Task<IActionResult> CreateEnergy(EnergyM energyM)
        {
            try
            {
                var dd = $"{RouteData.Values["controller"]}|{RouteData.Values["action"]}";
                EnergyDto energyDto = _energyMapper.ModelToEnergyDto(energyM);
                await _energyService.CreateEnergyAsync(energyDto);                
                string urlStr = Url.ActionContext.HttpContext.Request.Path.Value;
                
                var ddd = Request.Query;
                var dddd = Request.QueryString.Value;
                var sss = HttpContext.Request.QueryString.Value;
                var dsds = HttpContext;
                
                urlStr = $"{HttpContext.Request.Host.Value}" +
                    $"{HttpContext.Request.Path.Value}/{nameof(CreateEnergy)}" +
                    $"/{energyM.EnergyName}";
                
                return Created(urlStr, null);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
            
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateEnergy([FromBody] string name)
        //{
        //    if (name.Trim() == string.Empty)
        //        return BadRequest($"Name of empty");
        //    EnergyDto energyDto = new EnergyDto
        //    {
        //        Id = 0,
        //        EnergyName = name
        //    };
        //    if (await _energyService.UpsertEnergyTypeAsync(energyDto))
        //        return Ok($"The result of adding an energy type with name = {name} is Ok");
        //    return BadRequest($"The result of adding an energy type with name = {name} is failed");            
        //}
                
        [HttpDelete("RemoveEnergy/{id}")]
        public async Task<IActionResult> DeleteEnergy(int id)
        {
            var result = await _energyService.DeleteEnergyTypeByIdAsync(id);
            if (!result)
                return BadRequest($"Not deleting energy with id={id}");
            return Ok(result + $" Energy remove {result} with id={id}");
        }

        [HttpPatch("UpdateEnergy/{id}")]
        public async Task<IActionResult> UdateEnergy(int id, [FromBody] string name)
        {
            if (id == 0 && name.Trim() == string.Empty)
                return BadRequest($"Id equal zero or name of empty");
            EnergyDto energyDto = new EnergyDto
            {
                Id = id,
                EnergyName = name
            };                   
            if (await _energyService.UpsertEnergyPatchAsync(energyDto))
                return Ok($"The result of changing the name of the energy type with Id = {id} is Ok");
            return BadRequest($"The result of changing the name of the energy type with Id = {id} is failed");
        }

    }
}
