using Sucre_DataAccess.Entities;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucreParameterType: IDbSucre<ParameterType, int>, ISelectListItemObj
    {
        void Update(ParameterType parameterType);
        Task UpdateAsync(ParameterType parameterType);
    }
}
