using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_MVC.Filters;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_DataAccess.Services;
using Sucre_MVC.Models;
using Sucre_Utility;
using System.Diagnostics;

namespace Sucre_MVC.Controllers
{
    //[ResourceFilterController(0)]
    [ResourceFilterAsyncIE]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly InitApplicattionDbContext _initDb;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, InitApplicattionDbContext initDb,
                                IConfiguration configuration)
        {
            _logger = logger;
            _initDb = initDb;
            _configuration = configuration;

            //get db name from connect string
            var connstr = _configuration.GetConnectionString("DefaultConnection").ToString();
            string strDatabase = connstr.Split(';').ToList().FirstOrDefault(item => item.Contains("Database"));
            string result = strDatabase.Split('=').Last().ToString();
            
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"*->Run action Home-{nameof(Index)}");
            LoggerExternal.LoggerEx.Information($"*->Run action Home-{nameof(Index)}");

            var fff = HttpContext.User;

            return View();
        }

        [Authorize(Policy = WC.SupervisorPolicy)]
        public IActionResult InitDbData()
        {
            TempData["InitDb"] = null;
            string errMsg = "";
            try
            {
                if (_initDb.InitDbValue(out errMsg))
                {
                    TempData["InitDb"] = $"Ok";
                }
                else
                {
                    TempData["InitDb"] = $"The database tables have been initialized. The procedure was unsuccessful. Error: {errMsg}";
                }
                
            }
            catch (Exception ex) 
            {
                TempData["InitDb"] = $"The database tables have been initialized. The procedure was unsuccessful. Error: {ex.Message}";                                    
            }
            
            //return Ok(parameterType);
            return View(nameof(Index));
        }

        //[Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return View();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region lesson filter
        [ResourceFilterSimple]
        [ResourceFilterAsyncSimple]
        [TypeFilter(typeof(ResourceFilterAsyncLog))]
        //[ServiceFilter(typeof(ResourceFilterAsyncLog))]
        public async Task<IActionResult> FilterLook()
        {
            try
            {
                LoggerExternal.LoggerEx.Information($"*->{nameof(FilterLook)}");
                return Json("FilterLook->Json: Ok");

            }
            catch (Exception ex) 
            {
                LoggerExternal.LoggerEx.Error($"*->{nameof(FilterLook)}!!!");
                return BadRequest(ex.Message);
            }
        }
        [ResourceFilterAction(0)]
        public async Task<IActionResult> FilterGlCnAc()
        {
            try
            {
                LoggerExternal.LoggerEx.Information($"*->{nameof(FilterGlCnAc)}");
               /// var jj = HttpContext;
                return Json("FilterGlCnAc->Json: Ok");

            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->{nameof(FilterGlCnAc)}!!!");
                return BadRequest(ex.Message);
            }
        }

        [ResourceFilterAction(0)]
        [ActionFilterFirst]
        [ActionFiltesSpaceCleaner]
        //[ActionFiltesAsyncCheck]
        [HttpGet]
        public async Task<IActionResult> MethodForActionFilter()
        {
            try
            {
                LoggerExternal.LoggerEx.Information($"*->{nameof(MethodForActionFilter)}-Get");
                var model = new ModelForActionFilter
                {
                    Id = Guid.NewGuid(),
                };
                return View(model);
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->{nameof(MethodForActionFilter)}-Get!!!");
                return BadRequest(ex.Message);
            }
        }
        [ActionFiltesAsyncCheck]
        //[ActionFilterFirst]
        [HttpPost]
        public async Task<IActionResult> MethodForActionFilter(ModelForActionFilter model)
        {
            try
            {
                LoggerExternal.LoggerEx.Information($"*->{nameof(MethodForActionFilter)}-Post");
                if (ModelState.IsValid)
                {
                    return Json("ModelForActionFilter (Post)->Json: Model Ok");
                }
                else
                {
                    return Json("ModelForActionFilter (Post)->Json: Model Failed");
                };
                
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->{nameof(MethodForActionFilter)}-Post!!!");
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}