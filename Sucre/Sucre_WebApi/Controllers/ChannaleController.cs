using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_Services.Interfaces;
using Sucre_WebApi.Models;

namespace Sucre_WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChannaleController : Controller
    {
        private readonly ICanalService _canalService;
        private readonly IMediator _mediator;

        public ChannaleController(
            ICanalService canalService,
            IMediator mediator)
        {
            _canalService = canalService;
            _mediator = mediator;
        }

        [HttpGet("GetChannales/{fullName:bool}")] 
        public async Task<ActionResult> GetChannales (bool fullName=false) 
        {
            object channalesDto = null;
            if (!fullName)
            {
                channalesDto = new List<ChannaleCreateApiM>();
                var channales = await _canalService.GetListCannalesAsync();
                //if (channales == null)
                //    return NotFound($"Not found list channales metering");
                channalesDto = channales.ToList();
            }
            else
            {
                channalesDto = new List<CannaleFullDto>();
                //IEnumerable<CannaleFullDto> channalesFullDto = new HashSet<CannaleFullDto>();
                channalesDto = (await _canalService.GelListCannalesFullAsync(null)).ToList();

            }
            if (channalesDto == null)
                return NotFound($"Not found list channales metering");


            return Ok(channalesDto);

            

        }

        
        [HttpGet("GetChannale/{id}/{fullName:bool}")]
        public async Task<IActionResult> GetChannaleById(int id, bool fullName = false)
        {
            object channaleDto = null;
            if (!fullName) 
            {
                channaleDto = await _canalService.GetCannaleByIdAsync(id);
            }
            else
            {
                channaleDto = await _canalService.GetCannaleByIdFullAsync(id);
            }
            ///var channaleDto = await _canalService.GetCannaleByIdAsync(id);
            if (channaleDto == null) return NotFound($"Not found channale with Id={id}");
            return Ok(channaleDto);
        }

        [HttpGet("GetChannaleAssignedPoint/{id}/{fullName:bool}")]
        public async Task<IActionResult> GetChannaleByIdAndPoints(int id, bool fullName = false)
        {
            object channalePoints= null;
            if (!fullName)
            {
                //PointCanalesDto
                channalePoints = await _canalService.GetChannalePointesAsync(id);
                if (channalePoints == null) return NotFound($"Not found channale with Id={id}");
                return Ok(channalePoints);
            }
            else
            {
                //var
                channalePoints = await _canalService.GetChannalePointsFullAsync(id);
            }
            if (channalePoints == null) return NotFound($"Not found channale with Id={id}");
            return Ok(channalePoints);

        }

        [HttpPost]
        public async Task<IActionResult> CreateChannale([FromBody] ChannaleCreateApiM channaleCreateM)
        {
            string urlStr = string.Empty;
            if (channaleCreateM.ParameterTypeId  <= 0)
                ModelState.AddModelError("ParameterTypeId", "No correct field value");
            if (!ModelState.IsValid)
            {
                string strErrors = string.Empty;                
                foreach (var mdSt in ModelState)
                {
                    strErrors += $"{mdSt.Key.ToString()}: {mdSt.Value.Errors[0].ErrorMessage}";
                    strErrors += System.Environment.NewLine;
                };
                return BadRequest($"{strErrors}");
            };
            CanalDto channaleDto = new CanalDto
            {
                Id = 0,
                Name = channaleCreateM.Name,
                Description = channaleCreateM.Description,
                ParameterTypeId = channaleCreateM.ParameterTypeId,
                Reader = channaleCreateM.Reader,
                SourceType = channaleCreateM.SourceType,
                AsPazEin = channaleCreateM.AsPazEin
            };
            if (! await _canalService.CreateChannaleAsync(channaleDto))
            {
                return BadRequest($"Not creating object {channaleDto}");
            }
            urlStr = $"{HttpContext.Request.Host.Value}" +
                $"{HttpContext.Request.Path.Value}/" +
                $"{nameof(CreateChannale)}/{channaleCreateM.Name}";
            return Created(urlStr, null);
        }

        [HttpDelete("RemoveChannale/{id}")]
        public async Task<IActionResult> DeleteChannale(int id)
        {
            var result = await _canalService.DeleteChannaleByIdAsync(id);
            if (!result)
                return BadRequest($"Bad request for delete or not found with id={id}");
            return Ok($"Channale remove with id={id} Ok {result}");
        }

        [HttpPatch("UpdateChannale/{id}")]
        public async Task<IActionResult> UdateChannale(int id, [FromBody] CanalDto channaleDto)
        {
            if (id == 0)
                return BadRequest($"Id equal zero");
            if (channaleDto == null)
                return BadRequest($"The channaleDto object in the request is empty");
            if (channaleDto != null &&
                (channaleDto.Name.Trim() == string.Empty ||
                channaleDto.ParameterTypeId <= 0))
            {
                return BadRequest($"The fields of the channaleDto object in the request are empty");
            }
            if (await _canalService.UpsertCanalAsync(channaleDto))
                return Ok($"The result of changing the object the channale with Id = {id} is Ok");
            return BadRequest($"The result of changing the object of the channale with Id = {id} is failed");
        }

        [HttpPost]
        [Route("fire-and-forget")]
        public async Task<IActionResult>  FireAndForget(string client)
        {
            string jobId = BackgroundJob.Enqueue(() => _canalService.Z_HangFire(1, client));
            return Ok($"Job Id {jobId}");
        }

        [HttpPost]
        [Route("delayed")]
        public async Task<IActionResult> Delayed(string client)
        {
            string jobId = BackgroundJob
                .Schedule(
                    () => _canalService.Z_HangFire(2,client), 
                    TimeSpan.FromSeconds(60));
            return Ok($"Job Id {jobId}");
        }

        [HttpPost]
        [Route("recuring")]
        public async Task<IActionResult> Recurring()
        {
            RecurringJob.AddOrUpdate(
                "TTTT",
                () => _canalService.Z_HangFire(3, null),
                "1 0/1 * * *");
                
            return Ok();
        }

        [HttpGet]
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
            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> JobEin(string job)
        {
            if (job != "valuehour" && job != "valueday")
                return BadRequest($"{job} not in list jobs");
            bool findjob = false;
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();
                foreach (var recurringJob in recurringJobs)
                {
                    var dd = recurringJob;
                    if (job == dd.Id) 
                        findjob = true;
                }
            }
            if (findjob)
            {
                RecurringJob.RemoveIfExists(job);
            }
            else
            {
                if (job == "valuehour")
                {
                    RecurringJob.AddOrUpdate(
                        "valuehour",
                        () => _canalService.ReadValuesHour(),
                        "1 0/1 * * *");
                }
                if (job == "valueday")
                {
                    RecurringJob.AddOrUpdate(
                        "valueday",
                        () => _canalService.ReadValuesDay(),
                        "10 1 * * *");
                }
            }
            string res = findjob ? "in STOP" : "in EIN";
            return Ok($"{job} {res}");
        }

        //[HttpPost]
        //[Route("READER-CHANNALE/{id:int}")]
        //public async Task<IActionResult> ReaderChannale(int id, string? uri)
        //{
        //    await _canalService.ReadValueChannaleFromDevice(id, uri);
            
        //    //bool result = await _canalService.ReadValueChannaleFromDevice(uri);

        //    return Ok();
        //}

        //[HttpPost]
        //[Route("READER-ALL-DEVICE")]
        //public async Task<IActionResult> ReaderDevice(string? uri)
        //{
        //    RecurringJob.AddOrUpdate(
        //        "valuehour",
        //        () => _canalService.ReadValuesChannalesFromDevice(uri),
        //        "1 0/1 * * *");

        //    //bool result = await _canalService.ReadValueChannaleFromDevice(uri);

        //    return Ok();
        //}
    }    
}
