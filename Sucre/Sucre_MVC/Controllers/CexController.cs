using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_MVC.Controllers
{
    [Authorize]
    public class CexController : Controller
    {
        private readonly CexMapper _cexMapper;
        private readonly ICexService _cexService;        

        public CexController(CexMapper cexMapper, ICexService cexService)
        {
            _cexMapper = cexMapper;
            _cexService = cexService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cexsDto = await _cexService.GetListCexsAsync();            
            IEnumerable<CexM> cexsM = cexsDto
                .Select(cex => _cexMapper.CexDtoToModel(cex));            
            return View(cexsM);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? Id)
        {
            TempData["FieldIsEmpty"] = null;
            CexM cexM = new CexM();
            if (Id == null)
            {                
            }
            else
            {
                cexM = _cexMapper.CexDtoToModel(
                    await _cexService.GetCexByIdAsync(Id.GetValueOrDefault()));
                if (cexM == null)
                {
                    return NotFound($"Cex with Id = {Id.Value} not found");
                }
                else
                {                    
                }
            }
            return View(cexM);
        }

        [Authorize(Roles = $"{WC.AdminRole}, {WC.SupervisorRole}")]
        [HttpPost]
        public async Task<IActionResult> Upsert(CexM cexM)
        {
            TempData["FieldIsEmpty"] = null;
            if (ModelState.IsValid)
            {

                if ((cexM.Management == null || cexM.Management.Trim() == "") &&
                    (cexM.CexName == null || cexM.CexName.Trim() == "") &&
                    (cexM.Area == null || cexM.Area.Trim() == "") &&
                    (cexM.Device == null || cexM.Device.Trim() == "") &&
                    (cexM.Location == null || cexM.Location.Trim() == ""))
                {
                    TempData["FieldIsEmpty"] = "There is no record of the location of the metering point";
                    return View(cexM); ;
                }

                CexDto cexDto = _cexMapper.ModelToCexDto(cexM);
                if (cexM.Id == 0)
                {
                    //Creating                    
                }
                else
                {
                    //Update                                        
                }
                bool result = await _cexService.UpsertCexAsync(cexDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest("An error occurred while adding/updating an cex");
            }
            return View(cexM);
        }

        [Authorize(Roles = $"{WC.AdminRole}, {WC.SupervisorRole}")]
        //[Authorize(Policy = WC.SupervisorPolicy)]
        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0) return NotFound("Energy type ID not specified or ID equal to zero");
            CexDto cexDto = await _cexService.GetCexByIdAsync(Id.GetValueOrDefault());
            if (cexDto == null)
                return NotFound($"No cex with Id = {Id.Value}");
            CexM cexM = _cexMapper.CexDtoToModel(cexDto);
            cexM.FullName = WM.GetStringName(new string[]
            {
                cexM.Management,
                cexM.CexName,
                cexM.Area,
                cexM.Device,
                cexM.Location
            });
            return View(cexM);

            //Cex cex = await _sucreUnitOfWork.repoSucreCex.FirstOrDefaultAsync(filter: item => item.Id == Id.GetValueOrDefault());
            //if (cex == null) return NotFound(cex);
            //CexM cexM = new CexM();
            //sp_Cex(ref cex, ref cexM, true);
            ////cexM.FullName = _sucreUnitOfWork.repoSucreCex.FullName(cex);
            //cexM.FullName = _sucreUnitOfWork.repoSucreCex.GetStringName(cex);
            //return View(cexM);
        }

        [HttpPost]
        [Authorize(Roles = $"{WC.AdminRole}, {WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? Id)
        {
            if (Id == null || Id.GetValueOrDefault() == 0) return NotFound("Energy type ID not specified or ID equal to zero");
            bool result = await _cexService.DeleteCexByIdAsync(Id.Value);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest($"An error occurred while deleting an cex (Id == {Id.Value})");
            
        }

    }
}
