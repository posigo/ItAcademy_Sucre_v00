using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreAppUser: IDbSucre<AppUser, Guid>
    {
        //IEnumerable<SelectListItem> GetAllDropdownList(string strInclude);
        string GetStringName(object obj);
        void Update(AppUser appUser);
    }
}
