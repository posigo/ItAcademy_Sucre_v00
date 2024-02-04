using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreGroupUser: IDbSucre<GroupUser, int>, ISelectListItemObj
    {
        void Update(Energy energy);
        Task UpdateAsync(Energy energy);
    }
}
