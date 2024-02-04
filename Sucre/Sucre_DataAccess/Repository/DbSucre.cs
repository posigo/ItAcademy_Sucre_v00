using Microsoft.EntityFrameworkCore;
using Sucre_DataAccess.Data;
using Sucre_Core;
using Sucre_DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.Repository
{
    public class DbSucre<T, U> : IDbSucre<T, U> where T : class, IBaseEntity<U>
    {
        private readonly ApplicationDbContext _db;
        //private readonly ILogger<DbSucre<T>> _log;
        internal DbSet<T> dbSet;        

        public DbSucre(ApplicationDbContext db)
            //,
            //ILogger<DbSucre<T>> log = null)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            //_log = log;

            //log.LogInformation("Repository DbSucre use");
        }

        /// <summary>
        /// Add single entity
        /// </summary>
            /// <param name="entity">entity</param>
        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }
        /// <summary>
        /// Add single entity. Async method
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        /// <summary>
        /// Add multiple entities. Async method
        /// </summary>
        /// <param name="entities">entities</param>
        /// <returns></returns>
        public virtual async Task AddManyAsync(T entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// ? count. Async method
        /// </summary>
        /// <returns></returns>
        public async Task<int> Count()
        { 
            return await dbSet.CountAsync();
        }

        /// <summary>
        /// Search for an entity by ID
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <returns></returns>
        public virtual T Find(U id)
        {
            return dbSet.Find(id);
        }
        /// <summary>
        /// Search for an entity by ID. Async method
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <returns></returns>
        public virtual async Task<T> FindAsync(U id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Return an entity based on conditions
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking</param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProp);

            if (!isTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault();
        }
        /// <summary>
        /// Return an entity based on conditions. Async method
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking</param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProp);

            if (!isTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Select entities by conditions
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="orderBy">Order (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking in DB</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProp);

            if (orderBy != null)
                query = orderBy(query);

            if (!isTracking)
                query = query.AsNoTracking();

            return query.ToList();

        }
        /// <summary>
        /// Select entities by conditions. Async method
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="orderBy">Order (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking in DB</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, 
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                                                string includeProperties = null, 
                                                bool isTracking = true)
        {
            try
            {
                IQueryable<T> query = dbSet;
                if (filter != null)
                    query = query.Where(filter);

                if (includeProperties != null)
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        query = query.Include(includeProp);

                if (orderBy != null)
                    query = orderBy(query);

                if (!isTracking)
                    query = query.AsNoTracking();

                return await query.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new NotImplementedException(ex.Message);
                
            };
            return null;
        }

        public virtual IQueryable<T> GetAsQueryable()
        {
            return dbSet.AsQueryable();
        }

        public async Task<T?> GetById(U id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var resultQuery = dbSet.AsQueryable();
                if (includes.Any())
                {
                    resultQuery = includes.Aggregate(resultQuery, (current, include) => current.Include(include));
                }

                return await resultQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<T?> GetByIdAsNoTracking(U id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var resultQuery = dbSet.AsNoTracking();
                if (includes.Any())
                {
                    //resultQuery = includes.Aggregate(resultQuery, (current, include) => current.Include<T>(include.ToString()));
                    resultQuery = includes.Aggregate(resultQuery, (current, include) => current.Include(include));
                }

                return await resultQuery.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            
        }

        public async Task Patch(U id, List<PatchDto> patchDtos)
        {
            try
            {
                var entity = await GetById(id);
                if (entity != null)
                {
                    var nameValuePairProperties = patchDtos.ToDictionary(key => key.PropertyName,
                                                                        value => value.PropertyValue);
                    var dbEntityEntry = _db.Entry(entity);
                    dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
                    dbEntityEntry.State = EntityState.Modified;
                }
                else
                {
                    throw new ArgumentException("Incorrect Id for update", nameof(id));
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            
        }

        public virtual void Remove(T entity)
        {
            dbSet.Remove(entity);
        }        
        public virtual async Task RemoveAsync(T entity)
        {
            await Task.Run(() => Remove(entity));
        }
        public virtual async Task RemoveByIdAsync(U id)
        { 
            var entityRemove = await GetById(id);
            if (entityRemove != null)
            {
                dbSet.Remove(entityRemove);
            }
            else
            {
                throw new ArgumentException("Incorrect Id for delete", nameof(id));
            }
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                var entitiesToRemove = entities.Where(entity => dbSet.Any(dbe => dbe.Id.Equals(entity.Id))).ToList();
                dbSet.RemoveRange(entitiesToRemove);
            }
            
        }
        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => RemoveRange(entities));
        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //}
        //public async Task SaveAsync()
        //{
        //    await _db.SaveChangesAsync();
        //}

    }
}
