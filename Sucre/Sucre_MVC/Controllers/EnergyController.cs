using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_MVC.Filters;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Models;
using Sucre_Utility;
using Sucre_Services.Interfaces;
using Sucre_Core.DTOs;
using Sucre_Mappers;

namespace Sucre_MVC.Controllers
{
    //[SimpleResourceFilter]
    [ResourceFilterAsyncSimple]
    [FakeNotFoundResourceFilter] //не понял 
    [Authorize(Roles = $"{WC.SupervisorRole},{WC.AdminRole},{WC.UserRole}")]
    public class EnergyController : Controller
    {
        private readonly EnergyMapper _energyMapper;
        private readonly IEnergyService _energyService;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        

        public EnergyController(
            EnergyMapper energyMapper,
            IEnergyService energyService, 
            ISucreUnitOfWork sucreUnitOfWork)
        {
            _energyMapper = energyMapper;
            _energyService = energyService;
            _sucreUnitOfWork = sucreUnitOfWork;
            

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cont = HttpContext;
            string sdsd = cont.Request.Headers["User-Agent"].ToString();
           
            var energiesDto = await _energyService.GetListEnergyTypesAsync();
           
            IEnumerable<EnergyM> energiesM = energiesDto
                .Select(energyDto => _energyMapper.EnergyDtoToModel(energyDto));
            return View(energiesM);
        }
       
        [HttpGet]        
        public async Task<IActionResult> Upsert(int? Id)
        {
            EnergyM energyM = new EnergyM();
            if (Id == null || Id.Value == 0)
            {
                return View(energyM);
            }
            else
            {                
                EnergyDto energyDto = await _energyService.GetEnergyTypeByIdAsync(Id.GetValueOrDefault());
                if (energyDto == null)
                {
                    return NotFound($"No energy type with iD = {Id.GetValueOrDefault()}");
                }
                else
                {                    
                    energyM = _energyMapper.EnergyDtoToModel(energyDto);
             
                    return View(energyM);
                }
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EnergyM energyM)
        {
            if (ModelState.IsValid)
            {
                EnergyDto energyDto = new EnergyDto();
                energyDto = _energyMapper.ModelToEnergyDto(energyM);

                //Energy energy = new Energy();
                if (energyM.Id == null || energyM.Id == 0)
                {
                    //Creating
                    //energyDto.EnergyName = energyM.EnergyName;
                    energyDto.Id = 0;
                }
                else
                {
                    //Update
                    //energyDto.Id = energyM.Id;
                    //energyDto.EnergyName = energyM.EnergyName;

                }
                bool result = await _energyService.UpsertEnergyTypeAsync(energyDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }                
                return BadRequest("An error occurred while adding/updating an energy type");                
            }
            return View(energyM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0) return NotFound("Energy type ID not specified or ID equal to zero");
            var energyDto = await _energyService.GetEnergyTypeByIdAsync(Id.GetValueOrDefault());
            if (energyDto == null)
                return NotFound($"No energy type with Id = {Id.Value}");
            EnergyM energyM = _energyMapper.EnergyDtoToModel(energyDto);
            
            return View(energyM);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(EnergyM energyM,int? Id)
        {
            if (energyM == null)
                return BadRequest($"Energy type not found");
            EnergyDto energyDto = _energyMapper.ModelToEnergyDto(energyM);
            bool result = await _energyService.DeleteEnergyTypeAsync(energyDto);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest($"An error occurred while deleting an energy type (Id == {energyM.Id})");

            //if (Id == null || Id.Value == 0) return NotFound($"Energy type ID not specified or ID equal to zero");
            //bool res = await _energyService.DeleteEnergyTypeByIdAsync(Id.Value);
            //if (res)
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //return BadRequest($"An error occurred while deleting an energy type (Id == {Id.Value})");

        }
       
    }
}
