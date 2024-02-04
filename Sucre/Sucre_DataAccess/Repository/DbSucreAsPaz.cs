using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreAsPaz : DbSucre<AsPaz, int>, IDbSucreAsPaz
    {
        private readonly ApplicationDbContext _db;

        public DbSucreAsPaz(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(AsPaz asPaz)
        {
            _db.Update(asPaz);
        }
        public async Task UpdateAsync(AsPaz asPaz)
        {
            await Task.Run(() => Update(asPaz));
        }
    }
}
