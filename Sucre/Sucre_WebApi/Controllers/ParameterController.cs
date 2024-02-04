using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_Mappers;
using Sucre_Services.Interfaces;

namespace Sucre_WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly IParameterTypeService _parameterTypeService;
        private readonly ParameterTypeMapper _parameterTypeMapper;
       

        public ParameterController(
            IParameterTypeService parameterTypeService,
            ParameterTypeMapper parameterTypeMapper) 
        { 
            _parameterTypeMapper = parameterTypeMapper;
            _parameterTypeService = parameterTypeService;
        }

        [HttpGet("GetParameter/{id}")]
        public async Task<IActionResult> GetParameterById(int id)
        {
            //var energy = await _energyService.GetEnergyTypeByIdAsync(id);
            var parameterDto = await _parameterTypeService.GetParameterTypeByIdAsync(id);
            if (parameterDto == null) { return NotFound($"Not found type parameter with id={id}"); }
            return Ok(parameterDto);
        }

        [Authorize]
        [HttpGet("GetParameters")]
        public async Task<IActionResult> GetParameters()
        {
            var parameters = await _parameterTypeService.GetListParameterTypesAsync();
            if (parameters == null) { return NotFound("Not found"); }
            return Ok(parameters);
        }

        [HttpPost]
        [Route("NewParameter")]
        public async Task<IActionResult> CreateParameter(ParameterTypeDto parameterDto)
        {
            try
            {

                await _parameterTypeService.CreateParameterAsync(parameterDto);                
                string urlStr = Url.ActionContext.HttpContext.Request.Path.Value;
                
                
                urlStr = $"{HttpContext.Request.Host.Value}" +
                    $"{HttpContext.Request.Path.Value}/{nameof(CreateParameter)}" +
                    $"/{parameterDto.Name}";                
                return Created(urlStr, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
                        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParameter(int id)
        {
            var result = await _parameterTypeService.DeleteParameterTypeByIdAsync(id);
            if (!result)
                return BadRequest($"Not deleting parameter type with id={id}");
            return Ok(result + $" Parameter type remove {result} with id={id}");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UdpateParameter(int id, [FromBody] ParameterTypeDto parameterDto)
        {
            if (id == 0 || parameterDto == null)
                return BadRequest($"Id equal zero or object of empty");
                
            if (await _parameterTypeService.UpsertParameterPatchAsync(parameterDto))
                return Ok($"The result of changing the name of the parameter type with Id = {id} is Ok");
            return BadRequest($"The result of changing the name of the parameter type with Id = {id} is failed");
        }

    }
}
