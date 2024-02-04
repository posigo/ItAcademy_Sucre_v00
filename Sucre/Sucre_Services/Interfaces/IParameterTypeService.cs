using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IParameterTypeService
    {
        Task CreateParameterAsync(ParameterTypeDto parameterDto);
        Task<bool> DeleteParameterTypeAsync(ParameterTypeDto parameterTypeDtoo);
        Task<bool> DeleteParameterTypeByIdAsync(int Id);
        Task<ParameterTypeDto> GetParameterTypeByIdAsync(int Id);        
        Task<IEnumerable<ParameterTypeDto>> GetListParameterTypesAsync();
        IEnumerable<SelectListItem> GetParameterTypeSelectList(bool addFirstSelect = true,
            string valueFirstSelect = null);
        Task<bool> UpsertParameterPatchAsync(ParameterTypeDto parameterDto);
        Task<bool> UpsertParameterTypeAsync(ParameterTypeDto energyDto);
    }
}
