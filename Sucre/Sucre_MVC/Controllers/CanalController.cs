using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_MVC.Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_MVC.Controllers
{
    [Authorize]
    public class CanalController : Controller
    {
        private readonly IAsPazService _asPazService;
        private readonly ICanalService _canalService;
        private readonly IPointService _pointService;
        private readonly IParameterTypeService _parameterTypeService;
        private readonly AsPazMapper _asPazMapper;
        private readonly CanalMapper _canalMapper;
        private readonly PointMapper _pointMapper;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;

        public CanalController(
            IParameterTypeService parameterTypeService,
            ICanalService canalService,
            IAsPazService asPazService,
            IPointService pointService,
            AsPazMapper asPazMapper,
            CanalMapper canalMapper,
            PointMapper pointMapper)
        {
            _parameterTypeService = parameterTypeService;
            _canalService = canalService;
            _asPazMapper = asPazMapper;
            _canalMapper=canalMapper;
            _pointMapper = pointMapper;
            _asPazService = asPazService;
            _pointService = pointService;
            
        }

        #region Channales
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var cannalesFullDto = await _canalService.GelListCannalesFullAsync(null);

            //IEnumerable<CanalM> cannalesM = new HashSet<CanalM>();
            //foreach (var cannaleFullDto in cannalesFullDto)
            //{
            //    var cannaleM = _canalMapper.CannaleFullDtoToM(cannaleFullDto);
            //    cannalesM.Append(cannaleM);
            //}

            IEnumerable<CanalM> cannalesM = cannalesFullDto
                .Select(cannaleFullDto => _canalMapper.CannaleFullDtoToM(cannaleFullDto));

            return View(cannalesM);
            
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? Id)
        {
            CanalUpsertM canalUpsertM = new CanalUpsertM()
            {
                CanalM = new CanalM(),
                //ParametryTyoeSelectList = _canalDb.GetAllDropdownList(WC.ParameterTypeName)
                ParametryTypeSelectList = _parameterTypeService
                    .GetParameterTypeSelectList(valueFirstSelect: "--Select parameter type")
            };
            if (Id == null || Id.Value == 0)
            {
                return View(canalUpsertM);
            }
            else
            {
                CanalM canalM = _canalMapper.CanalDtoToModel(await _canalService.GetCannaleByIdAsync(Id.Value));
                
                if (canalM == null)
                {
                    return NotFound($"Channel with Id equal to {Id.GetValueOrDefault()} not found");
                }
                canalUpsertM.CanalM = canalM;
                return View(canalUpsertM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CanalM canalM)
        {
            if (ModelState.IsValid)
            {
                CanalDto canalDto = _canalMapper.ModelToCanalDto(canalM);
                if (canalM.Id == 0)
                {
                    //Creating 
                }
                else
                {
                    //Update
                    if (!canalM.AsPazEin)
                    {
                        int resAsPaz = await _asPazService
                            .CheckAndDelByChanaleIdAsync(canalM.Id);
                        if (resAsPaz == 1)
                        {
                            LoggerExternal.LoggerEx.Warning($"*-->CanalController, Not AsPaz from channale with Id = {canalM.Id}");
                        }
                        if (resAsPaz == 0)
                        {
                            LoggerExternal.LoggerEx.Error($"*-->CanalController, Error removing AsPaz from channel with Id = {canalM.Id}");
                            return BadRequest($"Error removing AsPaz from channel with Id = {canalM.Id}");
                        }
                    }
                }
                bool result = await _canalService.UpsertCanalAsync(canalDto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest($"Channel with Id esqual to {canalM.Id} not found");
                
            }
            return View(canalM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0) return NotFound("The passed ID is empty or equal to zero");
            //if (channaleM == null || 
            //    (channaleM != null && channaleM.Id == 0)) return NotFound("The channale of empty or passed ID is empty or equal to zero");
            CannaleFullDto channaleFullDto = await _canalService.GetCannaleByIdFullAsync(Id.Value);
            if (channaleFullDto != null)
            {
                CanalM channaleM = _canalMapper.CannaleFullDtoToM(channaleFullDto);
                return View(channaleM);
            }
            else
            {
                return NotFound($"Channel entity with ID = {Id.GetValueOrDefault()} not found");
            }         

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(CanalM channaleM)
        {
            //if (Id == null || Id == 0)
            //    return NotFound("The passed ID is empty or equal to zero");
            if (channaleM == null ||
                (channaleM != null && channaleM.Id == 0)) return NotFound("The channale of empty or passed ID is empty or equal to zero");
            if (channaleM.AsPazEin && !channaleM.AsPazEmpty)
            {
                int resAsPaz = await _asPazService
                    .CheckAndDelByChanaleIdAsync(channaleM.Id);
                if (resAsPaz == 0)
                {
                    LoggerExternal.LoggerEx.Error($"*-->CanalController, " +
                        $"Error removing AsPaz from channel with Id = {channaleM.Id}");
                    return BadRequest($"Error removing AsPaz from channel with Id = {channaleM.Id}");
                }
            }
            bool result = await _canalService
                .DeleteChannaleByIdAsync(channaleM.Id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest($"Channel with Id esqual to {channaleM.Id} not found or bad");
            
        }
        #endregion

        #region АСиПаз
        public async Task<IActionResult> IndexAsPaz()
        {
            try
            {                
                var asPazsChannaleDto = await _asPazService.GetListAsPasChannaleAsync();
                var asPazsChannaleM = asPazsChannaleDto
                    .Select(asPazChannaleDto => _asPazMapper
                    .AsPazChannaleDtoToModel(asPazChannaleDto));                
                return View(asPazsChannaleM);
                            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("I know now");
            
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsPaz(int? CanalId)
        {

            //var AsPazsDb = _asPazDb.GetAll(includeProperties: $"{WC.CanalName}");
            ICollection<AsPazCanalM> asPazCanalMs = new HashSet<AsPazCanalM>();

            ICollection<AsPaz> AsPazsDb = new HashSet<AsPaz>();
            
            IEnumerable<AsPazCanalM> asPazsChannaleM = new HashSet<AsPazCanalM>();
            List<AsPazCanalM> asPazlChannaleM = new List<AsPazCanalM>();


            if (CanalId == null || CanalId.GetValueOrDefault() == 0)            
            {
                var asPazsChannaleDto = await _asPazService.GetListAsPasChannaleAsync();
                asPazsChannaleM = asPazsChannaleDto
                    .Select(asPazChannaleDto => _asPazMapper
                    .AsPazChannaleDtoToModel(asPazChannaleDto));
                return View(asPazsChannaleM);
            }
            else
            {
                var asPazChannaleDto = await _asPazService.GetAsPazChannaleByIdCanAsync(CanalId.Value);
                var asPazChanneleM = _asPazMapper.AsPazChannaleDtoToModel(asPazChannaleDto);
                //List<AsPazCanalM> asPazsChanneleM  = new List<AsPazCanalM>();
                asPazlChannaleM.Add(asPazChanneleM);
                return View(asPazlChannaleM);
               
            }
            
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> UpsertAsPaz(int? Id, int? canalId, string? canalName)
        {
            if (canalId == null || canalId == 0)
            {
                return NotFound("Channel ID is empty or zero");
            }
            if (canalName == null) canalName = string.Empty;
            AsPazCanalM asPazCanalM = new AsPazCanalM()
            {
                AsPazM = new AsPazM()
                { 
                    CanalId = canalId.GetValueOrDefault() 
                },                
                CanalName = canalName
            };
            if (Id == null || Id.Value == 0)
            {
                return View(asPazCanalM);

            }
            else
            {
                var asPazDto = await _asPazService.GetAsPazByIdAsync(Id.Value);

                //AsPaz asPaz = await _sucreUnitOfWork.repoSucreAsPaz.FirstOrDefaultAsync(filter: item => item.Id == Id.Value &&
                                                                                   // item.CanalId == canalId.Value);
                if (asPazDto == null)
                {
                    return NotFound($"The AsPaz (Id = {Id.Value}) of a channel with Id equal to {canalId} was not found");
                }
                else
                {
                    asPazCanalM.AsPazM = _asPazMapper.AsPazDtoToModel(asPazDto);
                    
                    return View(asPazCanalM);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertAsPaz(AsPazCanalM asPazCanalM)
        {
            if (asPazCanalM.AsPazM.CanalId == null || asPazCanalM.AsPazM.CanalId == 0)
                return BadRequest("Channel ID is empty or zero");
            if (ModelState.IsValid)
            {
                AsPazDto asPazDto = _asPazMapper.ModelToAsPazDto(asPazCanalM.AsPazM);
                if (asPazCanalM.AsPazM.Id == 0)
                {
                    //Creating                    
                    //AsPazM asPazM = asPazCanalM.AsPazM;
                    //asPazM.CanalId = asPazCanalM.CanalId;
                    ////asPazM = asPazCanalM.AsPazM;
                    //sp_AsPaz(ref asPaz, ref asPazM, false);

                    //await _sucreUnitOfWork.repoSucreAsPaz.AddAsync(asPaz);
                }
                else
                {
                    //Update
                    //asPaz = _sucreUnitOfWork.repoSucreAsPaz.FirstOrDefault(filter: item => item.Id == asPazCanalM.AsPazM.Id && 
                    //                                                        item.CanalId == asPazCanalM.CanalId, isTracking: false);
                    //asPaz = await _sucreUnitOfWork.repoSucreAsPaz.FirstOrDefaultAsync(
                    //                                            filter: item => item.Id == asPazCanalM.AsPazM.Id &&
                    //                                            item.CanalId == asPazCanalM.CanalId, isTracking: false);
                    //if (asPaz == null)
                    //{
                    //    return NotFound($"The AsPaz of a channel with Id equal to {asPazCanalM.CanalId} was not found");
                    //}
                    //else
                    //{
                    //    AsPazM asPazM = asPazCanalM.AsPazM;
                    //    asPazM.CanalId = asPazCanalM.CanalId;
                    //    sp_AsPaz(ref asPaz, ref asPazM, false);
                    //    _sucreUnitOfWork.repoSucreAsPaz.Update(asPaz);
                    //}
                }
                bool result = await _asPazService.UpsertAsPazAsync(asPazDto);
                if (result)
                {
                    return RedirectToAction(nameof(IndexAsPaz));
                }
                return BadRequest($"AsPaz with Id esqual to {asPazDto.Id} for channale with Id = {asPazDto.CanalId} upsert failed");
            }

            return View(asPazCanalM);
        }

        //[HttpGet]
        public async Task<IActionResult> DeleteAsPaz(int? Id)
        {
            if (Id == null || Id.GetValueOrDefault() == 0) return BadRequest("Id AsPaz is empty or equal to zero");
            AsPazChannaleDto asPazChannaleDto = await _asPazService
                .GetAsPazChannaleByIdAsync(Id.Value);
            if (asPazChannaleDto == null) return NotFound(new String($"The AsPaz of a channel with ID equal to {Id.GetValueOrDefault()} was not found"));
            AsPazCanalM asPazCanalM = _asPazMapper.AsPazChannaleDtoToModel(asPazChannaleDto); 
                
            return View(asPazCanalM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAsPaz")]
        public async Task<IActionResult> DeleteAsPazPost(AsPazCanalM? asPazCanalM)
        {
            //if (Id == null || Id == 0) return NotFound("Id AsPaz is empty or equal to zero");
            if (asPazCanalM == null || 
                (asPazCanalM!=null && asPazCanalM.AsPazM.Id == 0) ||
                (asPazCanalM != null && asPazCanalM.AsPazM.CanalId == 0)) 
                return NotFound("Id AsPaz is empty or equal to zero");
            var canalDto = await _canalService.GetCannaleByIdAsync(asPazCanalM.AsPazM.CanalId);
            if (canalDto == null)
                return BadRequest($"Channale with Id = {asPazCanalM.AsPazM.CanalId} not found for" +
                    $"remove AsPaz with Id = {asPazCanalM.AsPazM.Id}");
            canalDto.AsPazEin = false;
            bool resChannale = await _canalService.UpsertCanalAsync(canalDto);
            if (!resChannale)
                return BadRequest($"Error upsert channale with Id = {asPazCanalM.AsPazM.CanalId} not found for" +
                    $"remove AsPaz with Id = {asPazCanalM.AsPazM.Id}");
            bool result = await _asPazService.DeleteAsPazByIdAsync(asPazCanalM.AsPazM.Id);
            if (result)
            {
                return RedirectToAction(nameof(IndexAsPaz));
            }
            return BadRequest($"Error remove AsPaz with Id = {asPazCanalM.AsPazM.Id}");

        }

        #endregion

        #region Точки учёта
        [HttpGet]
        public async Task<IActionResult> ChannalePointsIndex(int? Id)
        {                        
            if (Id == null || Id.Value == 0)
                return BadRequest("Id null or equal zero");

             ChannalePointsDto channalePointsDto = await _canalService
                .GetChannalePointesAsync(Id.GetValueOrDefault());
            
            if (channalePointsDto == null)
                return NotFound($"Channale with Id = {Id.Value} not found");
            
            CannalePointsM channalePointsM = new CannalePointsM();
            channalePointsM.Id = channalePointsDto.ChannaleDto.Id;
            channalePointsM.Name = channalePointsDto.ChannaleDto.Name;

            channalePointsM.PointsM = channalePointsDto.PointsDto
                .Select(pointDto => _pointMapper.PointDtoToModel(pointDto));
                        
            List<int> listIdPointsAssigned = channalePointsM.PointsM
                .Select(pointM => pointM.Id).ToList(); // new List<int>();

            List<PointShortNameDto> pointsShortNameDto =  _pointService
                .GetListPointsForId(listIdPointsAssigned, false, true);

            
            channalePointsM.FreePointsSelectList = pointsShortNameDto
                .Select(pointShorName => new SelectListItem
                {
                    Text = $"{pointShorName.Id}, {pointShorName.Name}, {pointShorName.EnergyName}, {pointShorName.CexName}",
                    Value = $"{pointShorName.Id}"
                });

            return View(channalePointsM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CannalePointsDelete(int Id, int IdPoint, CannalePointsM cannalePointsM)
        {
            bool result = await _canalService.UpsertPointToCanal(Id, IdPoint);
            if (result)
            {
                return RedirectToAction(nameof(ChannalePointsIndex), new { Id = Id });
            }
            return BadRequest($"Deleting a metering point Id = {IdPoint} from a channale with ID = {Id} error");
                        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CannalePointsAdding(int Id, int Add, CannalePointsM cannalePointsM)
        {
            if (cannalePointsM.AddPoint == 0)
                return RedirectToAction(nameof(ChannalePointsIndex), new { Id = Id });
            int AddIdPoint = cannalePointsM.AddPoint;

            bool result = await _canalService.UpsertPointToCanal(Id, AddIdPoint, true);
            if (result)
            {
                return RedirectToAction(nameof(ChannalePointsIndex), new { Id = Id });
            }
            return BadRequest($"Adding a metering point Id = {AddIdPoint} from a channale with ID = {Id} error");
                          
        }
        #endregion 


        //[Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        //public async Task<IActionResult> ReadAllFromApi(int? ein)
        //{
        //    if ((ein.HasValue && ein.Value == 3) || (!ein.HasValue))
        //    {
        //        await _canalService.ReadValuesHour();
        //    }                
        //    if (ein.HasValue && ein.Value == 1)
        //    {
        //        RecurringJob.AddOrUpdate(
        //            "valuehour",
        //            () => _canalService.ReadValuesHour(),
        //            "1 0/1 * * *");
        //    }
        //    if (ein.HasValue && ein.Value == 0)
        //    {
        //        //RecurringJob.
        //        //RecurringJob.AddOrUpdate(
        //        //    "valuehour",
        //        //    () => _canalService.ReadValuesHour(),
        //        //    "* * * * *");
        //        RecurringJob.RemoveIfExists("valuehour");
        //    }
        //    return Ok();
        //}
        
        //[Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        //public async Task<IActionResult> ReadChannaleByIdHour(int id, DateTime dateTime, int? hour)
        //{
        //    //_canalService.ReadValuesHourMan(8, Convert.ToDateTime("12.01.2024").Date, null);
        //    await _canalService.ReadValuesHourMan(id, dateTime.Date, hour);

        //    return Ok();
        //}

        [HttpGet]
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> ReadManHour()
        {
            ManReadM manReadM = new ManReadM();
            return View(manReadM);
        }

        [HttpPost]
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadManHourPost(ManReadM manReadM)
        {
            if (manReadM.IdChannale == 0)
            {
                ModelState.AddModelError("IdChannale", "Id Channale is Zero");
            }
            DateTime dateTimeB;

            if (ModelState.IsValid)
            {
                bool resDate = DateTime.TryParse($"{manReadM.DayB}.{manReadM.MonthB}.{manReadM.YearB}", out dateTimeB);
                if (!resDate || (resDate && dateTimeB>DateTime.Now)) 
                {
                    manReadM.Result = "Date is FAILED";
                    return View("ReadManHour", manReadM);
                }
                bool result = await _canalService.ReadValuesHourMan(
                    manReadM.IdChannale,
                    Convert.ToDateTime($"{manReadM.DayB}.{manReadM.MonthB}.{manReadM.YearB}"),
                    manReadM.Hour.HasValue?manReadM.Hour.Value:null); 
                if (result)
                {
                    ManReadM manRead = new ManReadM();
                    manRead.Result = "Read is OK";
                    return View("ReadManHour", manRead);
                }
                {
                    manReadM.Result = "Read is FAILED";
                    return View("ReadManHour", manReadM);
                }
            }
            manReadM.Result = "";
            return View("ReadManHour", manReadM);
        }
        
        //[Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        //public async Task<IActionResult> ReadAllDFromApi(int? ein)
        //{
        //    if ((ein.HasValue && ein.Value == 3) || (!ein.HasValue))
        //    {
        //        await _canalService.ReadValuesDay();
        //    }
        //    if (ein.HasValue && ein.Value == 1)
        //    {
        //        RecurringJob.AddOrUpdate(
        //            "valueday",
        //            () => _canalService.ReadValuesHour(),
        //            "10 1 * * *");
        //    }
        //    if (ein.HasValue && ein.Value == 0)
        //    {
        //        //RecurringJob.
        //        //RecurringJob.AddOrUpdate(
        //        //    "valuehour",
        //        //    () => _canalService.ReadValuesHour(),
        //        //    "* * * * *");
        //        RecurringJob.RemoveIfExists("valueday");
                
                
        //    }
        //    return Ok();
        //}
        
        //[Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        //public async Task<IActionResult> ReadChannaleByIdDay(int id, DateTime dateTime, DateTime? dateTimee)
        //{
        //    //_canalService.ReadValuesHourMan(8, Convert.ToDateTime("12.01.2024").Date, null);
        //    await _canalService.ReadValuesDayMan(id, dateTime.Date, dateTimee);

        //    return Ok();
        //}

        [HttpGet]
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> ReadManDay()
        {
            ManReadM manReadM = new ManReadM();
            //manReadM.YearE = 0;
            //manReadM.MonthE = 0;
            //manReadM.DayE = 0;
            return View(manReadM);
        }
       
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadManDayPost(ManReadM manReadM)
        {
            
            DateTime dateTimeE;
            DateTime dateTimeB;

            if (ModelState.IsValid)
            {
                bool resultDateB = DateTime.TryParse($"{manReadM.DayB}.{manReadM.MonthB}.{manReadM.YearB}", out dateTimeB);
                if (!resultDateB || (resultDateB && dateTimeB>DateTime.Now))
                {
                    manReadM.Result = "Date begin is FAILED";
                    return View("ReadManDay", manReadM);
                }
                bool resultDate = DateTime.TryParse($"{manReadM.DayE}.{manReadM.MonthE}.{manReadM.YearE}", out dateTimeE);
                if (resultDate && dateTimeE>DateTime.Now)
                {
                    manReadM.Result = "Date end is FAILED";
                    return View("ReadManDay", manReadM);
                }
                bool result = await _canalService.ReadValuesDayMan(
                    manReadM.IdChannale,
                    Convert.ToDateTime($"{manReadM.DayB}.{manReadM.MonthB}.{manReadM.YearB}"),
                    resultDate ? dateTimeE : null);
                if (result)
                {
                    ManReadM manRead = new ManReadM();
                    manRead.Result = "Read is OK";
                    return View("ReadManDay", manRead);
                }
                else
                {
                    manReadM.Result = "Read is FAILED";
                    return View("ReadManDay", manReadM);
                }
            }
            manReadM.Result = "";
            return View("ReadManDay", manReadM);
        }

        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> GetListJobs()
        {
            Dictionary<string, bool> jobsd = new Dictionary<string, bool>()
                {
                    {"valuehour", false },
                    {"valueday", false }
                };

            List<JobStatus> jobs = new List<JobStatus>();
                
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();
                foreach (var recurringJob in recurringJobs)
                {
                    var dd = recurringJob;
                    if (jobsd.ContainsKey(dd.Id))
                    { jobsd[dd.Id] = true; }
                        //if (NonRemovableJobs.ContainsKey(recurringJob.Id)) continue;
                    //logger.LogWarning($"Removing job with id [{recurringJob.Id}]");
                    //jobManager.RemoveIfExists(recurringJob.Id);
                }
            }

            foreach (var job in jobsd) 
            {
                jobs.Add(new() { JobId = job.Key, Ein = job.Value });
            }
            return View(jobs);
            //return Ok();
        }
        [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
        public async Task<IActionResult> Jobsrun(string id, bool ein)
        {
            switch (id)
            {
                case "valuehour":
                    if (ein)
                    {
                        RecurringJob.RemoveIfExists("valuehour");
                    }
                    else
                    {
                        RecurringJob.AddOrUpdate(
                            "valuehour",
                            () => _canalService.ReadValuesHour(),
                            "1 0/1 * * *");

                    };
                    break;
                case "valueday":
                    if (ein)
                    {
                        RecurringJob.RemoveIfExists("valueday");
                    }
                    else
                    {
                        RecurringJob.AddOrUpdate(
                            "valueday",
                            () => _canalService.ReadValuesDay(),
                            "10 1 * * *");
                    }
                    break;
            }
            return RedirectToAction(nameof(GetListJobs));
            //return Ok();
        }
    }
    
}
