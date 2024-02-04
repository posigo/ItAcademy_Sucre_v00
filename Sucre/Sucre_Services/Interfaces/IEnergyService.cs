using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IEnergyService
    {
        Task CreateEnergyAsync(EnergyDto energyDto);
        Task<bool> DeleteEnergyTypeAsync(EnergyDto energyDto);
        Task<bool> DeleteEnergyTypeByIdAsync(int Id);
        /// <summary>
        /// Get Dto Energy
        /// </summary>
        /// <param name="Id">Id type Energy</param>
        /// <returns></returns>
        Task<EnergyDto> GetEnergyByIdAsync(int Id);
        Task<EnergyDto> GetEnergyTypeByIdAsync(int Id);
        Task<IEnumerable<EnergyDto>> GetListEnergyTypesAsync();
        IEnumerable<SelectListItem> GetEnergySelectList(bool addFirstSelect = true, 
            string valueFirstSelect = null);
        Task<bool> UpsertEnergyPatchAsync(EnergyDto energyDto);
        Task<bool> UpsertEnergyTypeAsync(EnergyDto energyDto);
    }
}
