using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;

namespace Sucre_Services
{
    public class EnergyService:IEnergyService
    {
        private readonly IConfiguration _configuration;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly EnergyMapper _energyMapper;
        private readonly IMediator _mediator;

        public EnergyService(IConfiguration configuration,
            ISucreUnitOfWork sucreUnitOfWork,
            EnergyMapper energyMapper,
            IMediator mediator)
        {
            _configuration = configuration;
            _sucreUnitOfWork = sucreUnitOfWork;
            _energyMapper = energyMapper;
            _mediator = mediator; 
        }

        public async Task CreateEnergyAsync(EnergyDto energyDto)
        {
            var command = new AddEnergyCommand()
            {
                EnergyDto = energyDto
            };            
            await _mediator.Send(command);            
        }

        public async Task<bool> DeleteEnergyTypeAsync(EnergyDto energyDto)
        {
            try
            {
                if (energyDto == null) return false;
                //Energy energy = new Energy()
                //{
                //    Id = energyDto.Id,
                //    EnergyName = energyDto.EnergyName
                //};
                Energy energy = _energyMapper.EnergyDtoToEnergy(energyDto);
                await _sucreUnitOfWork.repoSucreEnergy.RemoveAsync(energy);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, EnergyService->{nameof(DeleteEnergyTypeAsync)}");
            }
            return false;
        }

        public async Task<bool> DeleteEnergyTypeByIdAsync(int Id)
        {
            try
            {
                if (Id == 0) return false;
                await _sucreUnitOfWork.repoSucreEnergy.RemoveByIdAsync(Id);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, EnergyService->{nameof(DeleteEnergyTypeByIdAsync)}");
            }
            return false;
        }

        public async Task<EnergyDto> GetEnergyByIdAsync(int Id)
        {
            try
            {
                var energy = await _mediator.Send(new GetEnergyByIdQuery() { Id = Id });
                //   );
                return (energy!=null)? _energyMapper.EnergyToEnergyDto(energy):null;

            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, EnergyService->{nameof(GetEnergyByIdAsync)}");
            }
            return null;
        }
        
        public async Task<EnergyDto> GetEnergyTypeByIdAsync(int Id)
        {
            try
            {
                var energyDb = await _sucreUnitOfWork.repoSucreEnergy.FindAsync(Id);
                //if (energyDb == null) { return null; }
                //EnergyDto energyDto = new EnergyDto
                //{
                //    Id = energyDb.Id,
                //    EnergyName = energyDb.EnergyName
                //};
                //EnergyDto energyDto = _energyMapper.EnergyToEnergyDto(energyDb);
                //return energyDto;
                return (energyDb != null)
                    ? _energyMapper.EnergyToEnergyDto(energyDb)
                    : null;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, EnergyService->{nameof(GetEnergyTypeByIdAsync)}");
            }
            return null;
        }

        public async Task<IEnumerable<EnergyDto>> GetListEnergyTypesAsync()
        {
            try
            {
                var energiesDb = await _sucreUnitOfWork.repoSucreEnergy.GetAllAsync();
                //IEnumerable<EnergyDto> energiesDto = energiesDb
                //    .Select(energyDb => new EnergyDto
                //    {
                //        Id = energyDb.Id,
                //        EnergyName = energyDb.EnergyName
                //    });
                IEnumerable<EnergyDto> energiesDto = energiesDb
                    .Select(energyDb => _energyMapper.EnergyToEnergyDto(energyDb));
                return energiesDto;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"*-->Error, EnergyService->{nameof(GetListEnergyTypesAsync)}");
            }
            return null;
        }

        /// <summary>
        /// Получить список элементов объекта
        /// </summary>
        /// <param name="AddFirstSelect">Добавить первый элемент</param>
        /// <param name="valueFirstSelect">Название первого элемента</param>
        /// <returns>список элементов</returns>
        public IEnumerable<SelectListItem> GetEnergySelectList(bool addFirstSelect = true, 
            string valueFirstSelect = null)
        {
            return _sucreUnitOfWork.repoSucreEnergy.GetAllDropdownList(addFirstSelect, valueFirstSelect);
        }

        public async Task<bool> UpsertEnergyPatchAsync(EnergyDto energyDto)
        {
            try
            {
                if (energyDto == null) { return false; }
                Energy energy = _energyMapper.EnergyDtoToEnergy(energyDto);                
                List<PatchDto> patchs = new List<PatchDto>()
                {
                    new()
                    {
                        PropertyName = nameof(energy.EnergyName),
                        PropertyValue = energy.EnergyName
                    }
                };
                await _sucreUnitOfWork.repoSucreEnergy.Patch(energy.Id, patchs);
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, EnergyService->{nameof(UpsertEnergyPatchAsync)}");
            }

            return false;
        }

        public async Task<bool> UpsertEnergyTypeAsync(EnergyDto energyDto)
        {
            try
            {
                if (energyDto == null) { return false; }
                Energy energy = new Energy();
                energy = _energyMapper.EnergyDtoToEnergy(energyDto);
                //energy.EnergyName = energyDto.EnergyName;
                if (energyDto.Id == null || energyDto.Id == 0)
                {
                    await _sucreUnitOfWork.repoSucreEnergy.AddAsync(energy);
                }
                else
                {
                    //energy.Id = energyDto.Id;
                    await _sucreUnitOfWork.repoSucreEnergy.UpdateAsync(energy);

                    //List<PatchDto> patchs = new List<PatchDto>()
                    //{
                    //    new()
                    //    {
                    //        PropertyName = nameof(energy.EnergyName),
                    //        PropertyValue = energy.EnergyName
                    //    }
                    //};
                    //await _sucreUnitOfWork.repoSucreEnergy.Patch(energy.Id, patchs);
                }
                await _sucreUnitOfWork.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                LoggerExternal.LoggerEx.Error(ex, $"!!!-->Error, EnergyService->{nameof(UpsertEnergyTypeAsync)}");
            }
            
            return false;
        }
    }
}
