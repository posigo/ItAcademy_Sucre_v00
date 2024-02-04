using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface ICexService
    {
        Task<bool> CreateCexAsync(CexDto cexDto);
        Task<bool> DeleteCexAsync(CexDto cexDto);
        Task<bool> DeleteCexByIdAsync(int Id);        
        Task<CexDto> GetCexByIdAsync(int Id);
        Task<CexPointsCanalsDto> GetCexPointsChanalesById(int id);
        IEnumerable<SelectListItem> GetCexSelectList(bool addFirstSelect = true,
            string valueFirstSelect = null);
        Task<IEnumerable<CexDto>> GetListCexsAsync();
        Task<bool> UpsertCexPatchAsync(CexDto cexDto);
        Task<bool> UpsertCexAsync(CexDto cexDto);
    }
}
