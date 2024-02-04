using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreAppRole: IDbSucre<AppRole, Guid>
    {        
        void Update(AppRole appRole);
    }
}
