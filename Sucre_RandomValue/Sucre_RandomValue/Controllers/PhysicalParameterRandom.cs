using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Sucre_RandomValue.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    public class PhysicalParameterRandom : ControllerBase
    {
        private readonly ILogger<PhysicalParameterRandom> _logger;
        private readonly IConfiguration _configuration;
        private readonly List<string> _enviroment = new List<string>()
        {
            "gas","nitrogen","compressedAir","water","steam","warn"
        };
        private readonly List<string> _type = new List<string>()
        {
            "temperature","pressure","consumption"
        };
        private readonly Dictionary<int, string> _enviroment2 = new Dictionary<int, string>()
        {
            { 1, "gas" },
            { 2, "nitrogen"},
            { 3, "compressedAir" },
            { 4, "water" },
            { 5, "steam" },
            { 6, "warn" }
        };
        private readonly Dictionary<int, string> _parameter = new Dictionary<int, string>()
        {
            { 154, "temperature" },
            { 156, "pressure"},
            { 159, "consumption" }
        };

        public PhysicalParameterRandom(
            ILogger<PhysicalParameterRandom> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [NonAction]
        private string name(int id, List<string> inlst)
        {
            foreach (var item in inlst)
            {
                var str = _configuration.GetSection(item).Value;
                var lst = str.ToString().Split(",");
                var fnd = lst.FirstOrDefault(d => d == id.ToString());
                if (fnd != null)
                {
                    return item.ToString();
                    //enviroment = item.ToString();
                    //break;
                }                
            }
            return null;
        }

        [HttpGet]
        public async Task<double> GetRandomValue(int id)
        {
            try
            {
                string enviroment = name(id, _enviroment);// string.Empty;
                string type = name(id, _type); // string.Empty;

                //foreach (var item in _enviroment)
                //{
                //    var str = _configuration.GetSection(item).Value;
                //    var lst = str.ToString().Split(",");
                //    var fnd = lst.FirstOrDefault(d => d == id.ToString());
                //    if (fnd!=null)
                //    {
                //        enviroment = item.ToString();
                //        break;
                //    }
                //}
                //foreach (var item in _type)
                //{
                //    var str = _configuration.GetSection(item).Value;
                //    var lst = (_configuration.GetSection(item).Value)
                //        .ToString()
                //        .Split(",");
                //    var fnd = lst.FirstOrDefault(d => d == id.ToString());
                //    if (fnd != null)
                //    {
                //        type = item.ToString();
                //        break;
                //    }
                //}

                if (enviroment == string.Empty ||
                    enviroment == null ||
                    type == string.Empty ||
                    type == null)
                    return -999999.9;

                var dictionary = _configuration.GetSection("Options")
                    .GetChildren()
                    .ToDictionary(section => section.Key,
                        section => Convert.ToDouble(section.Value));
                //string key_min = String.Concat(enviroment, "_", phis, "_min");
                //string key_max = String.Concat(enviroment, "_", phis, "_max");
                string key_min = String.Concat(enviroment, "_", type, "_min");
                string key_max = String.Concat(enviroment, "_", type, "_max");
                double min = dictionary[key_min];
                double max = dictionary[key_max];
                int random = Random.Shared
                    .Next(int.Parse((min * 10).ToString())
                        , int.Parse((max * 10).ToString()));
                double result = double.Parse(random.ToString()) / 10;

                return result;
            }
            catch (Exception ex)
            {
                return 0.0;
            }
            
        }

        [HttpGet]
        public async Task<double> GetRandomValue2(int evn, int prm)
        {
            try
            {
                string enviroment = _enviroment2[evn];// string.Empty;
                string type = _parameter[prm]; // string.Empty;

               

                if (enviroment == string.Empty ||
                    enviroment == null ||
                    type == string.Empty ||
                    type == null)
                    return -999999.9;

                var dictionary = _configuration.GetSection("Options2")
                    .GetChildren()
                    .ToDictionary(section => section.Key,
                        section => Convert.ToDouble(section.Value));
                //string key_min = String.Concat(enviroment, "_", phis, "_min");
                //string key_max = String.Concat(enviroment, "_", phis, "_max");
                string key_min = String.Concat(enviroment, "_", type, "_min");
                string key_max = String.Concat(enviroment, "_", type, "_max");
                double min = dictionary[key_min];
                double max = dictionary[key_max];
                int random = Random.Shared
                    .Next(int.Parse((min * 10).ToString())
                        , int.Parse((max * 10).ToString()));
                double result = double.Parse(random.ToString()) / 10;

                return result;
            }
            catch (Exception ex)
            {
                return 0.0;
            }

        }
    }
}
