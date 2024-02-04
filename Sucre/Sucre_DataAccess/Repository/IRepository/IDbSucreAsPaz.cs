using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreAsPaz : IDbSucre<AsPaz, int>
    {
        void Update(AsPaz asPaz);
        Task UpdateAsync(AsPaz asPaz);
    }
}
