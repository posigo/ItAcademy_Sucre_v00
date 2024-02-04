using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucrePoint: IDbSucre<Point, int>
    {
        //IEnumerable<SelectListItem> GetAllDropdownList(string strInclude);
        //string GetStringCex(Cex cex);
        void Update(Point point);
    }
}
