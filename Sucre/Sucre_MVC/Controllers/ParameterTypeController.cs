using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Models;
using Sucre_Services.Interfaces;
using Sucre_Utility;

namespace Sucre_MVC.Controllers
{
    [Authorize]
    public class ParameterTypeController : Controller
    {
        private readonly ParameterTypeMapper _parameterTypeMapper;
        private readonly IParameterTypeService _parameterTypeService;
        //private readonly IDbSucreParameterType _parameterTypeDb;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly ILogger<ParameterTypeController> _log;

        public ParameterTypeController(
            ParameterTypeMapper parameterTypeMapper,
            IParameterTypeService parameterTypeService,
            ISucreUnitOfWork sucreUnitOfWork,
            ILogger<ParameterTypeController> log)
        {
            //_parameterTypeDb = parameterTypeDb;
            _parameterTypeMapper = parameterTypeMapper;
            _parameterTypeService = parameterTypeService;
            _sucreUnitOfWork = sucreUnitOfWork;     
            _log = log;

        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var parameterTypesDto = await _parameterTypeService.GetListParameterTypesAsync();
            IEnumerable<ParameterTypeM> parameterTypesM = parameterTypesDto
                .Select(parameter => _parameterTypeMapper
                .ParameterDtoToModel(parameter));
            return View(parameterTypesM);
        }

        [HttpGet]
        public IActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ParameterTypeM parameterTypeM)
        {
            if (ModelState.IsValid)
            {
                ParameterType parameterType = new ParameterType
                {
                    Id = parameterTypeM.Id,
                    Name = parameterTypeM.Name,
                    Mnemo = parameterTypeM.Mnemo,
                    UnitMeas = parameterTypeM.UnitMeas
                };
                await _sucreUnitOfWork.repoSucreParameterType.AddAsync(parameterType);
                //_parameterTypeDb.Add(parameterType);
                await _sucreUnitOfWork.CommitAsync();
                //_parameterTypeDb.Save();
                return RedirectToAction(nameof(Index));
            }
            return View (parameterTypeM);
        }

        
        [HttpGet]
        public async Task<IActionResult> Upsert(int? Id)
        {
            ParameterTypeM parameterTypeM = new ParameterTypeM();            
            if (Id == null || Id.Value == 0)
            {
                return View(parameterTypeM);
            }
            else
            {
                //ParameterType parameterType = await _parameterTypeDb.FindAsync(Id.GetValueOrDefault());
                ParameterType parameterType = await _sucreUnitOfWork.repoSucreParameterType.FindAsync(Id.GetValueOrDefault());
                ParameterTypeDto parameterTypeDto = await
                    _parameterTypeService.GetParameterTypeByIdAsync(Id.GetValueOrDefault());
                if (parameterTypeDto == null)
                {
                    return NotFound($"No parameter type with Id = {Id.Value}");
                }
                else
                {
                    parameterTypeM = _parameterTypeMapper
                        .ParameterDtoToModel(parameterTypeDto);
                    
                    return View(parameterTypeM);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ParameterTypeM parameterTypeM)
        {
            if (ModelState.IsValid)
            {
                ParameterType parameterType = new ParameterType();
                ParameterTypeDto parameterTypeDto = _parameterTypeMapper
                    .ModelToParameterDto(parameterTypeM);
                if (parameterTypeM.Id == null || parameterTypeM.Id == 0)
                {
                    parameterTypeDto.Id = 0;
                }
                else
                {
                    //Update
                }
                bool result = await _parameterTypeService
                    .UpsertParameterTypeAsync(parameterTypeDto);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest("An error occurred while adding/updating an parameter type");
                
            }
            return View(parameterTypeM);
        }

        [HttpGet]        
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id.Value == 0) return NotFound("Id is not specified or Id is zero");
            //ParameterType parameterType = _parameterTypeDb.FirstOrDefault(filter: item => item.Id == Id.GetValueOrDefault());
            ParameterTypeDto parameterTypeDto = await _parameterTypeService
                .GetParameterTypeByIdAsync(Id.GetValueOrDefault());  
        
            if (parameterTypeDto == null) return NotFound($"Parameter type with Id = {Id.Value} not found");
            ParameterTypeDelM parameterTypeDelM = new ParameterTypeDelM()
            {
                Id = parameterTypeDto.Id,
                
                FullName = WM.GetStringName( new string[]
                {
                        parameterTypeDto.Name,
                        parameterTypeDto.Mnemo,
                        parameterTypeDto.UnitMeas
                })
            };
            return View(parameterTypeDelM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost (int? Id)
        {
            //if (Id == null || Id == 0) return NotFound();
            ////var parameterType = _parameterTypeDb.Find(Id.GetValueOrDefault());
            //var parameterType = await _sucreUnitOfWork.repoSucreParameterType.FindAsync(Id.GetValueOrDefault());
            //if (parameterType == null) return NotFound(parameterType);
            ////_parameterTypeDb.Remove(parameterType);
            ////_parameterTypeDb.Save();
            //_sucreUnitOfWork.repoSucreParameterType.Remove(parameterType);
            //await _sucreUnitOfWork.CommitAsync();
            //return RedirectToAction(nameof(Index));
            ////return View();


            if (Id == null || Id.Value == 0) return NotFound($"ID not specified or ID equal to zero");
            bool res = await _parameterTypeService.DeleteParameterTypeByIdAsync(Id.Value);
            if (res)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest($"An error occurred while deleting an parameter type (Id == {Id.Value})");


        }
    }
}
