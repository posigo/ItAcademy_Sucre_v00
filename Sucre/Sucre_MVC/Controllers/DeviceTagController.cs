using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_MVC.Controllers
{
    [Authorize(Roles = $"{WC.AdminRole},{WC.SupervisorRole}")]
    public class DeviceTagController : Controller
    {
        private readonly IDeviceTagService _deviceTagService;
        private readonly ICanalService _canalService;
        private readonly DeviceMapper _deviceMapper;
        public DeviceTagController(
            DeviceMapper deviceMapper,
            IDeviceTagService deviceTagService,
            ICanalService canalService)
        {
            _deviceMapper = deviceMapper;
            _canalService = canalService;
            _deviceTagService = deviceTagService;
        }
        public async Task<IActionResult> GetDevices()
        {   
            List<DeviceDto> devicesDto = await _deviceTagService.GetDevices();


            return View(devicesDto);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? Id)
        {
            DeviceDto deviceDto = new DeviceDto();
            if (Id == null || Id.Value == 0)
            {
                return View(deviceDto);
            }
            else
            {
                    deviceDto = await _deviceTagService.GetDeviceDtoById(Id.Value);
                
                if (deviceDto == null)
                {
                    return NotFound($"Device with Id equal to {Id.GetValueOrDefault()} not found");
                }
                
                return View(deviceDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DeviceDto deviceDto)
        {
            if (ModelState.IsValid)
            {
                bool result = false;
                if (deviceDto.Id == 0)
                {
                    //Creating 
                    result = await _deviceTagService.CreateDevice(deviceDto);
                }
                else
                {
                    //Update
                    result = await _deviceTagService.PatchDevice(deviceDto);
                }                 
                if (result)
                {
                    return RedirectToAction(nameof(GetDevices));
                }
                return BadRequest($"Device with Id esqual to {deviceDto.Id} not found");

            }
            return View(deviceDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id.GetValueOrDefault() == 0)
                return NotFound("Device ID not specified or ID equal to zero");
            DeviceDto deviceDto = await _deviceTagService.GetDeviceDtoById(id.Value);            
            if (deviceDto == null) return NotFound($"No device with Id = {id.Value}");

            return View(deviceDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null || id.GetValueOrDefault() == 0) 
                return NotFound("Device ID not specified or ID equal to zero");
            bool result = await _deviceTagService.DeleteDeviceById(id.Value);
            if (result)
            {
                return RedirectToAction(nameof(GetDevices));
            }
            return BadRequest($"An error occurred while deleting an devise (Id == {id.Value})");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetTags()
        //{
        //    List<DeviceTagDto> tagsDto = await _deviceTagService.GetTags();


        //    return View(tagsDto);
        //}

        [HttpGet]
        public async Task<IActionResult> TableTagsDevice(int id)
        {
            
            if (id == 0) return BadRequest("id in zero");
            var viewM = new DeviceTagsM();
            var deviceTags = await _deviceTagService.GetTagsByDevice(id);
            viewM.Id = deviceTags.DeviceDto.Id;
            viewM.NameDevice = deviceTags.DeviceDto.Name;
            var paramselect = new List<SelectListItem>()
            {
                new() { Text = "Enter code parameter", Value = "0",
                    Selected = true, Disabled = true },
                new() { Text = "Temperature", Value ="154" },
                new() { Text = "Pressure", Value = "156" },
                new() { Text = "Comnsumption", Value="159" },
            };
            var enviromentSelect = new List<SelectListItem>()
            {
                new() { Text = "Enter code eviroment", Value = "0",
                    Selected = true, Disabled = true },
                new() { Text = "gas", Value ="1" },
                new() { Text = "nitrogen", Value ="2" },
                new() { Text = "compressedAir", Value ="3" },
                new() { Text = "water", Value ="4" },
                new() { Text = "steam", Value ="5" },
                new() { Text = "warn", Value ="6" },
            };
            var channalesFree = new List<SelectListItem>();
            var channale = await _canalService.GetListCannalesAsync();
            var channales = new List<SelectListItem>()
            {
                new() 
                { 
                    Text = "Select channale",
                    Value = "0",
                    Selected = true,
                    Disabled = true,
                }
            };            
            foreach (var item in channale)
            {
                var selLI = new SelectListItem()
                {
                    Text = $"{item.Name}",
                    Value = item.Id.ToString()
                };
                channales.Add(selLI);
                var ffind = deviceTags.Tags.FirstOrDefault(i => i.Id== item.Id);
                if (ffind == null)
                    channalesFree.Add(
                        new SelectListItem()
                        {
                            Text = $"{item.Id} {item.Name}",
                            Value = item.Id.ToString()
                        });
            };
            var tagMList = new List<TagM>();
            foreach(var item in deviceTags.Tags) 
            {
                var tagM = new TagM();
                tagM = _deviceMapper.DeviceTagDtoToTagM(item);
                tagM.ChannalesTypeSelectList = channales;
                tagM.EnviromentsTypeSelectList = enviromentSelect;
                tagM.ParametersTypeSelectList = paramselect;
                tagMList.Add(tagM);
            };
            viewM.TagsM = tagMList;
            viewM.FreeTagsM = channalesFree;

            return View(viewM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TagUpdate(int id, int idCannale, int env, int prm, DeviceTagsM deviceTagsM)
        {
            TagM tagForUpdate = new TagM()
            {
                DeviceId = id,
                ChannaleId = idCannale,
            };
            var newCollecTag = deviceTagsM.TagsM
                .Where(item => item.ChannaleId == idCannale)            
                .Select(model => new
                {
                    environment = model.Enviroment,
                    parameterCode = model.ParameterCode
                });
            tagForUpdate.Enviroment = newCollecTag
                .Select(i => i.environment)
                .FirstOrDefault();
            tagForUpdate.ParameterCode = newCollecTag
                .Select(i => i.parameterCode)
                .FirstOrDefault();
            DeviceTagDto tagDto = _deviceMapper.TagMToDeviceTagDto(tagForUpdate);
            await _deviceTagService.UpdateDeviceTag(tagDto);
            return RedirectToAction(nameof(TableTagsDevice), new { id = id });
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TagAdd(int id, DeviceTagsM deviceTagsM)
        {
            if (id == 0 || deviceTagsM.AddChannale == 0)
                return BadRequest("Id device equal zero or Id adding channale with to zero");
            TagM tagForUpdate = new TagM()
            {
                DeviceId = id,
                ChannaleId = deviceTagsM.AddChannale,
                Enviroment = 0,
                ParameterCode = 0
            };
            DeviceTagDto tagDto = _deviceMapper.TagMToDeviceTagDto(tagForUpdate);
            await _deviceTagService.CreateTag(tagDto);
            return RedirectToAction(nameof(TableTagsDevice), new { id = id });
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TagRemove(int id, int idCannale, DeviceTagsM deviceTagsM)
        {
            TagM tagForUpdate = new TagM()
            {
                DeviceId = id,
                ChannaleId = idCannale,
            };
            DeviceTagDto tagDto = _deviceMapper.TagMToDeviceTagDto(tagForUpdate);
            await _deviceTagService.RemoveTag(tagDto);
            return RedirectToAction(nameof(TableTagsDevice), new { id = id });
            

        }
    }

    
}
