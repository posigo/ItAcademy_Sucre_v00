using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreMethodList
    {
        IEnumerable<SelectListItem> GetAllDropdownList(string strInclude);
        string GetStringName(object obj);
    }
}
