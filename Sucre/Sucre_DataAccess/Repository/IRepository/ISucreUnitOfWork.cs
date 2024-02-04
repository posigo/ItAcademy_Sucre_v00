namespace Sucre_DataAccess.Repository.IRepository
{
    public interface ISucreUnitOfWork: IDisposable
    {
        IDbSucreAppRole repoSucreAppRole
        { get; }
        IDbSucreAppUser repoSucreAppUser
        { get; }
        IDbSucreAsPaz repoSucreAsPaz
        { get; }
        IDbSucreCanal repoSucreCanal
        { get; }
        IDbSucreCex repoSucreCex
        { get; }
        IDbSucreEnergy repoSucreEnergy
        { get; }
        IDbSucreGroupUser repoSucreGroupUser
        { get; }
        IDbSucreParameterType repoSucreParameterType
        { get; }
        IDbSucrePoint repoSucrePoint
        { get; }

        void Commit();
        Task CommitAsync();
    }
}
