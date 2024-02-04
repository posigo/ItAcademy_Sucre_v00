using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;

namespace Sucre_Services
{
    public class ParameterTypeService: IParameterTypeService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly ParameterTypeMapper _parameterTypeMapper;

        public ParameterTypeService(IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            ParameterTypeMapper parameterTypeMapper)
        {
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _parameterTypeMapper = parameterTypeMapper;
        }

        public async Task CreateParameterAsync(ParameterTypeDto parameterDto)
        {
            var command = new AddParameterCommand()
            {
                parameterDto = parameterDto             
            };
            await _mediator.Send(command);
        }

        public async Task<bool> DeleteParameterTypeAsync(ParameterTypeDto parameterTypeDto)
        {
            try
            {
                if (parameterTypeDto == null) return false;
                //Energy energy = new Energy()
                //{
                //    Id = energyDto.Id,
                //    EnergyName = energyDto.EnergyName
                //};
                ParameterType parameterType = _parameterTypeMapper.ParameterDtoToParameter(parameterTypeDto);
                await _sucreUnitOfWork.repoSucreParameterType.RemoveAsync(parameterType);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, ParameterTypeService->{nameof(DeleteParameterTypeAsync)}");
            }
            return false;
        }

        public async Task<bool> DeleteParameterTypeByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucreParameterType.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, ParameterTypeService-> {nameof(DeleteParameterTypeByIdAsync)}");
            }
            return false;
        }

        public async Task<ParameterTypeDto> GetParameterTypeByIdAsync(int Id)
        {
            try
            {
                var parameterTypeDb = await _sucreUnitOfWork.repoSucreParameterType.FindAsync(Id);
                //if (energyDb == null) { return null; }
                //EnergyDto energyDto = new EnergyDto
                //{
                //    Id = energyDb.Id,
                //    EnergyName = energyDb.EnergyName
                //};
                //EnergyDto energyDto = _energyMapper.EnergyToEnergyDto(energyDb);
                //return energyDto;
                return (parameterTypeDb != null)
                    ? _parameterTypeMapper.ParameterToParameterDto(parameterTypeDb)
                    : null;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, ParameterTypeService-> {nameof(GetParameterTypeByIdAsync)}");
            }
            return null;
        }

        public async Task<IEnumerable<ParameterTypeDto>> GetListParameterTypesAsync()
        {
            try
            {
                var parameterTypeDb = await _sucreUnitOfWork.repoSucreParameterType.GetAllAsync();
                //IEnumerable<EnergyDto> energiesDto = energiesDb
                //    .Select(energyDb => new EnergyDto
                //    {
                //        Id = energyDb.Id,
                //        EnergyName = energyDb.EnergyName
                //    });
                IEnumerable<ParameterTypeDto> parameterTypeDto = parameterTypeDb
                    .Select(parameter => _parameterTypeMapper.ParameterToParameterDto(parameter));
                return parameterTypeDto;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, ParameterTypeService->  {nameof(GetListParameterTypesAsync)}");
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetParameterTypeSelectList(bool addFirstSelect = true,
            string valueFirstSelect = null)
        {
            return _sucreUnitOfWork.repoSucreParameterType.GetAllDropdownList(addFirstSelect, valueFirstSelect);
        }

        public async Task<bool> UpsertParameterPatchAsync(ParameterTypeDto parameterDto)
        {
            try
            {
                if (parameterDto == null) { return false; }
                ParameterType parameter = _parameterTypeMapper.ParameterDtoToParameter(parameterDto);
                
                List<PatchDto> patchs = new List<PatchDto>()
                {
                    new() { PropertyName = nameof(parameter.Name), PropertyValue = parameter.Name },
                    new() { PropertyName = nameof(parameter.Mnemo), PropertyValue = parameter.Mnemo },
                    new() { PropertyName = nameof(parameter.UnitMeas), PropertyValue = parameter.UnitMeas }                    
                };
                await _sucreUnitOfWork.repoSucreParameterType.Patch(parameter.Id, patchs);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, CexService->{nameof(UpsertParameterPatchAsync)}");
            }

            return false;
        }

        public async Task<bool> UpsertParameterTypeAsync(ParameterTypeDto parameterTypeDto)
        {
            try
            {
                if (parameterTypeDto == null) { return false; }
                ParameterType parameterType = new ParameterType();
                parameterType = _parameterTypeMapper.ParameterDtoToParameter(parameterTypeDto);
                //energy.EnergyName = energyDto.EnergyName;
                if (parameterTypeDto.Id == null || parameterTypeDto.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucreParameterType.AddAsync(parameterType);
                }
                else
                {
                    //energy.Id = energyDto.Id;
                    await _sucreUnitOfWork.repoSucreParameterType.UpdateAsync(parameterType);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, ParameterTypeService->   {nameof(UpsertParameterTypeAsync)}");
            }
            
            return false;
        }

    }
}
