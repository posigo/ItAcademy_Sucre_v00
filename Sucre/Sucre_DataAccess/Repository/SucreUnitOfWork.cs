using Microsoft.Extensions.Logging;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class SucreUnitOfWork : ISucreUnitOfWork
    {
        private ApplicationDbContext _dbSucre;
        private readonly IDbSucreAppRole _repoSucreAppRole;
        private readonly IDbSucreAppUser _repoSucreAppUser;
        private readonly IDbSucreAsPaz _repoSucreAsPaz;
        private readonly IDbSucreCanal _repoSucreCanal;
        private readonly IDbSucreCex _repoSucreCex;
        private readonly IDbSucreEnergy _repoSucreEnergy;
        private readonly IDbSucreGroupUser _repoSucreGroupUser;
        private readonly IDbSucreParameterType _repoSucreParameterType;
        private readonly IDbSucrePoint _repoSucrePoint;
        private readonly ILogger<SucreUnitOfWork> _log;

        public SucreUnitOfWork(ApplicationDbContext dbSucre,
                            IDbSucreAppRole repoSucreAppRole,
                            IDbSucreAppUser repoSucreAppUser,
                            IDbSucreAsPaz repoSucreAsPaz,
                            IDbSucreCanal repoSucreCanal,
                            IDbSucreCex repoSucreCex,
                            IDbSucreEnergy repoSucreEnergy,
                            IDbSucreGroupUser repoSucreGroupUser,
                            IDbSucreParameterType repoSucreParameterType,
                            IDbSucrePoint repoSucrePoint,
                            ILogger<SucreUnitOfWork> log)
        {
            _dbSucre = dbSucre;
            _repoSucreAppRole = repoSucreAppRole;
            _repoSucreAppUser = repoSucreAppUser;
            _repoSucreAsPaz = repoSucreAsPaz;
            _repoSucreCanal = repoSucreCanal;
            _repoSucreCex = repoSucreCex;
            _repoSucreEnergy = repoSucreEnergy;
            _repoSucreGroupUser = repoSucreGroupUser;
            _repoSucreParameterType = repoSucreParameterType;
            _repoSucrePoint = repoSucrePoint ;
            _log = log;
            _log.LogInformation("SucreUnitOfWork use");
            LoggerExternal.LoggerEx.Information("*->SucreUnitOfWork use");
        }

        public IDbSucreAppRole repoSucreAppRole => _repoSucreAppRole;

        public IDbSucreAppUser repoSucreAppUser => _repoSucreAppUser;

        public IDbSucreAsPaz repoSucreAsPaz => _repoSucreAsPaz;

        public IDbSucreCanal repoSucreCanal => _repoSucreCanal;

        public IDbSucreCex repoSucreCex => _repoSucreCex;

        public IDbSucreEnergy repoSucreEnergy
        {
            get { return _repoSucreEnergy; }
        }

        public IDbSucreGroupUser repoSucreGroupUser => _repoSucreGroupUser;

        public IDbSucreParameterType repoSucreParameterType
        {
            get { return _repoSucreParameterType; }
        }

        public IDbSucrePoint repoSucrePoint
        {
            get { return _repoSucrePoint;}
        }
        
        public void Commit() => _dbSucre.SaveChanges();
        public async Task CommitAsync() => await _dbSucre.SaveChangesAsync();

        private bool disposed = false;
        public virtual void Dispose(bool disposing) 
        { 
            if (!this.disposed)
            {
                if (disposing) 
                    _dbSucre.Dispose();
                this.disposed = true;
            }

        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }
    }
}
