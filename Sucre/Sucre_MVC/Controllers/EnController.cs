using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_MVC.Filters;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Models;
using Sucre_Utility;
using Sucre_Services.Interfaces;
using Sucre_Core.DTOs;
using Sucre_Mappers;
using NuGet.Protocol;

namespace Sucre_MVC.Controllers
{
    //[SimpleResourceFilter]
    
    public class EnController : Controller
    {
        private readonly EnergyMapper _energyMapper;
        private readonly IEnergyService _energyService;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        

        public EnController(
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

            try
            {
                using (var client = new HttpClient())
                {
                    //https://localhost:7178/api/users
                    //https://localhost:7127/api/Users

                    client.BaseAddress = new Uri("https://localhost:7178/api/users");
                    var resp = await client.GetStringAsync("https://localhost:7178/api/users");
                    var resp2 = await client.GetAsync("https://localhost:7178/api/users");
                    List<Person> persons = new List<Person>();
                    var resp3 = await client.GetFromJsonAsync("https://localhost:7127/api/Users", typeof(List<Person>));
                    return Ok(resp3);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("good");
        }
       
        
    }

    public class Person
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }
}
