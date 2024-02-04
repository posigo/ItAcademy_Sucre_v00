using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreCex: IDbSucre<Cex, int>, ISelectListItemObj
    {
        //string FullName(Cex cex);
        void Update(Cex cex);
        Task UpdateAsync(Cex cex);
    }
}
