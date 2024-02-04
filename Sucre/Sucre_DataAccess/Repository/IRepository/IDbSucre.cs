//using Sucre_DataAccess.Entities.TDO;
using Sucre_Core;
using System.Linq.Expressions;
using Sucre_Core.DTOs;

namespace Sucre_DataAccess.Repository.IRepository
{
    public interface IDbSucre<T, U> where T : class, IBaseEntity<U>
    {
        /// <summary>
        /// Add single entity
        /// </summary>
        /// <param name="entity">entity</param>
        void Add(T entity);
        /// <summary>
        /// Add single entity. Async method
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        Task AddAsync(T entity);
        /// <summary>
        /// Add multiple entities. Async method
        /// </summary>
        /// <param name="entities">entities</param>
        /// <returns></returns>
        Task AddManyAsync(T entities);

        /// <summary>
        /// ? count. Async method
        /// </summary>
        /// <returns></returns>
        Task<int> Count();

        /// <summary>
        /// Search for an entity by ID
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <returns></returns>
        T Find(U id);
        /// <summary>
        /// Search for an entity by ID. Async method
        /// </summary>
        /// <param name="id">ID entity</param>
        /// <returns></returns>
        Task<T> FindAsync(U id);

        /// <summary>
        /// Return an entity based on conditions
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking</param>
        /// <returns></returns>
        T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true);
        /// <summary>
        /// Return an entity based on conditions. Async method
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking in DB</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true);

        /// <summary>
        /// Select entities by conditions
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="orderBy">Order (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking in DB</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true);
        /// <summary>
        /// Select entities by conditions. Async method
        /// </summary>
        /// <param name="filter">Filter (expression)</param>
        /// <param name="orderBy">Order (expression)</param>
        /// <param name="includeProperties">Pluggable entities</param>
        /// <param name="isTracking">Tracking in DB</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true);

        IQueryable<T> GetAsQueryable();

        Task<T?> GetById(U id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsNoTracking(U id, params Expression<Func<T, object>>[] includes);

        Task Patch(U id, List<PatchDto> patchTdos);

        void Remove(T entity);
        Task RemoveAsync(T entity);
        Task RemoveByIdAsync(U id);
        void RemoveRange(IEnumerable<T> entities);
        Task RemoveRangeAsync(IEnumerable<T> entities);

        //void Save();
        //Task SaveAsync();
    }
}
