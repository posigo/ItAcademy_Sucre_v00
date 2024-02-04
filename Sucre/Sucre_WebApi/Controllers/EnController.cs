//using Microsoft.AspNetCore.Mvc;
//using Sucre_Services;

//namespace Sucre_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EnController : Controller
//    {
//        private readonly EnService _EnService;

//        public EnController(EnService enService)
//        {
//            _EnService = enService;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            _EnService.Rate();
//            return Ok("EnService");
//        }
//    }
//}
