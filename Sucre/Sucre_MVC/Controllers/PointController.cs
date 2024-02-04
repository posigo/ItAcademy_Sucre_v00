using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Models;
using Microsoft.AspNetCore.Authorization;
using Sucre_Core.DTOs;
using Sucre_Services.Interfaces;
using Sucre_Mappers;

namespace Sucre_MVC.Controllers
{
    [Authorize]
    public class PointController : Controller
    {
        private readonly ICanalService _canalService;
        private readonly ICexService _cexService;
        private readonly IEnergyService _energyService;
        private readonly IPointService _pointService;
        private readonly CanalMapper _canalMapper;
        private readonly PointMapper _pointMapper;


        public PointController(
            ICanalService canalService,
            ICexService exceService,
            IEnergyService energyService,            
            IPointService pointService,
            CanalMapper canalMapper,
            PointMapper pointMapper)
        {
            _canalService = canalService;
            _cexService = exceService;
            _energyService = energyService;
            _pointMapper = pointMapper;
            _pointService = pointService;
            _canalMapper = canalMapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            IEnumerable<PointTableDto> pointsTableDto = await _pointService.GetListPointsByStrAsync();
            IEnumerable<PointTableM> pointsTableM = pointsTableDto
                .Select(pointTableDto => 
                _pointMapper.PointTableDtoToModel(pointTableDto));
            return View(pointsTableM);           
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? Id)
        {
            PointUpsertM pointUpsertM = new PointUpsertM()
            {
                PointM = new PointM(),
                EnergySelectList = _energyService.GetEnergySelectList(
                    valueFirstSelect: "--Select energy type--"), 
                CexSelectList = _cexService.GetCexSelectList(
                              valueFirstSelect: "--Select the location of the metering point--")
            };

            if (Id == null)
            {
                return View(pointUpsertM);
            }
            else
            {

                PointDto pointDto = await _pointService.GetPointByIdAsync(Id.GetValueOrDefault());
                if (pointDto == null)
                {
                    return NotFound($"No metering point with iD = {Id.GetValueOrDefault()}");
                }
                else
                {
                    pointUpsertM.PointM = _pointMapper.PointDtoToModel(pointDto);
                    return View(pointUpsertM);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PointM pointM)
        {
            if (ModelState.IsValid)
            {
                PointDto pointDto = _pointMapper.ModelToPointDto(pointM);
                if (pointM.Id == 0)
                {
                    //Creating                    
                    pointDto.Id = 0;
                }
                else
                {
                    //Update
                    //point = await _sucreUnitOfWork.repoSucrePoint.FirstOrDefaultAsync(isTracking: false,
                    //                                                    filter: item => item.Id == pointM.Id);
                    //if (point == null)
                    //{
                    //    return NotFound(point);
                    //}
                    //else
                    //{
                    //    sp_Point(ref point, ref pointM, false);
                    //    await _sucreUnitOfWork.repoSucrePoint.Patch(point.Id, new List<PatchTdo>()
                    //    {
                    //        new() {PropertyName = nameof(point.Name),PropertyValue = point.Name},
                    //        new() {PropertyName = nameof(point.Description),PropertyValue = point.Description},
                    //        new() {PropertyName = nameof(point.EnergyId),PropertyValue = point.EnergyId},
                    //        new() {PropertyName = nameof(point.CexId),PropertyValue = point.CexId},
                    //        new() {PropertyName = nameof(point.ServiceStaff),PropertyValue = point.ServiceStaff},

                    //    });

                    //    //_sucreUnitOfWork.repoSucrePoint.Update(point);
                   // }
                }
                bool result = await _pointService.UpsertPointAsync(pointDto);
                //_sucreUnitOfWork.Commit();
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest("An error occurred while adding/updating an metering point");
            }
            return View(pointM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id.GetValueOrDefault() == 0) 
                return NotFound("Metering point ID not specified or ID equal to zero");
            PointTableM pointTableM = _pointMapper.PointTableDtoToModel(
                await _pointService.GetPointByIdStrAsync(Id.Value));
                      
            if (pointTableM == null) return NotFound($"No metering point with Id = {Id.Value}");

            return View(pointTableM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? Id)
        {
            if (Id == null || Id.GetValueOrDefault() == 0) return NotFound("Metering point ID not specified or ID equal to zero");
            bool result = await _pointService.DeletePointByIdAsync(Id.Value);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest($"An error occurred while deleting an Metering point (Id == {Id.Value})");            
        }

        #region Каналы
        [HttpGet]
        public async Task<IActionResult> PointCannalesIndex(int? Id)
        {
            //var pointCanalesDb = _sukreUnitOfWork.repoSukrePoint.GetAll(filter: item => item.Id == Id.Value,
            //                                                         includeProperties: WC.CanalsName);
            if (Id == null || Id.Value == 0)
                return BadRequest("Id null or equal zero");

            PointCanalesDto pointCanalesDto = await _pointService.GetPointCanalesAsync(Id.Value);
            if (pointCanalesDto == null)
                return NotFound($"Metering point with Id = {Id.Value} not found"); 
            PointCannalesM pointCannalesM = new PointCannalesM();
            pointCannalesM.Id = pointCanalesDto.PointDto.Id;
            pointCannalesM.Name = pointCanalesDto.PointDto.Name;
            pointCannalesM.CannalesM = pointCanalesDto.CanalesDto
                .Select(canalDto => _canalMapper.CanalDtoToModel(canalDto));

            List<int> listIdCannalesAssigned = pointCannalesM.CannalesM
                .Select(canalM => canalM.Id).ToList(); // new List<int>();

            List<CanalShortNameDto> cannalesShortNameDto = _canalService
                .GetListCanalesForId(listIdCannalesAssigned, false, true);

            //List<CanalShortNameDto> cannalesShortNameDto1 = _canalService
            //    .GetListCanalesForId(listIdCannalesAssigned, false, false);
            //List<CanalShortNameDto> cannalesShortNameDto2 = _canalService
            //    .GetListCanalesForId(listIdCannalesAssigned, true, false);
            //List<CanalShortNameDto> cannalesShortNameDto3 = _canalService
            //    .GetListCanalesForId(listIdCannalesAssigned, true, true);

            pointCannalesM.FreeCanalesSelectList = cannalesShortNameDto
                .Select(canalShorName => new SelectListItem
                {
                    Text = $"{canalShorName.Id}, {canalShorName.Name}, {canalShorName.ParameterTypeName}",
                    Value = $"{canalShorName.Id}"
                });

            return View(pointCannalesM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PointCannalesDelete(int Id, int IdCannale, PointCannalesM pointCannalesM)
        {
            bool result = await _pointService.UpsertCanalToPoint(Id, IdCannale);
            if (result)
            {
                return RedirectToAction(nameof(PointCannalesIndex), new { Id = Id });
            }
            return BadRequest($"Deleting a channel Id = {IdCannale} from a metering point with ID = {Id} error");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PointCannalesAdding(int Id, int Add, PointCannalesM pointCannalesM)
        {

            if (pointCannalesM.AddCannale == 0)
                return RedirectToAction(nameof(PointCannalesIndex), new { Id = Id });
            int AddIdCannale = pointCannalesM.AddCannale;
                        
            bool result = await _pointService.UpsertCanalToPoint(Id, AddIdCannale, true);
            if (result)
            {
                return RedirectToAction(nameof(PointCannalesIndex), new { Id = Id });
            }
            return BadRequest($"Adding a channel Id = {AddIdCannale} from a metering point with ID = {Id} error");

        }
        #endregion

    }
}
