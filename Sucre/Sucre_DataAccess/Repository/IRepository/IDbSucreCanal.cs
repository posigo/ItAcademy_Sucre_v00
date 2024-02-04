using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreCanal: IDbSucre<Canal, int>
    {
        //IEnumerable<SelectListItem> GetAllDropdownList(string strInclude);
        //string GetStringCanal(ParameterType parameterType);
        void Update(Canal canal);
        Task UpdateAsync(Canal canal);
    }
}
