using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IPointService
    {
        /// <summary>
        /// Create new point
        /// </summary>
        /// <param name="pointDto">Entity Point</param>
        /// <returns></returns>
        Task<bool> CreatePointAsync(PointDto pointDto);
        /// <summary>
        /// Delete point
        /// </summary>
        /// <param name="pointDto"></param>
        /// <returns></returns>
        Task<bool> DeletePointAsync(PointDto pointDto);
        /// <summary>
        /// Deleting a given entity with Id
        /// </summary>
        /// <param name="Id">Id entity point</param>
        /// <returns></returns>
        Task<bool> DeletePointByIdAsync(int Id);
        /// <summary>
        /// Get entity point with Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<PointDto> GetPointByIdAsync(int Id);
        /// <summary>
        /// Getting entities with full names and given ID
        /// </summary>
        /// <param name="id">Id entity point</param>
        /// <returns></returns>
        Task<PointTableDto> GetPointsFullByIdAsync(int id);
        /// <summary>
        /// Get list points with full name 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PointTableDto>> GetListPointsByStrAsync();
        /// <summary>
        /// Getting entities with full names and given ID for MVC
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<PointTableDto> GetPointByIdStrAsync(int Id);
        /// <summary>
        /// Get a point by given ID and linked channels
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PointCanalesDto> GetPointCanalesAsync(int id);
        Task<PointChannalesFullDto> GetPointCanalesFullAsync(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listIds"></param>
        /// <param name="tEqual"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        List<PointShortNameDto> GetListPointsForId(
            List<int> listIds = null,
            bool tEqual = false,
            bool paramName = true);
        /// <summary>
        /// Getting entities with full names
        /// </summary>
        /// <returns></returns>
        Task<List<PointTableDto>> GetPointsFullAsync();
        /// <summary>
        /// Get list points
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PointDto>> GetListPointsAsync();
        /// <summary>
        /// Update/Add channales to point with Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IdCannale"></param>
        /// <param name="upsert"></param>
        /// <returns></returns>
        Task<bool> UpsertCanalToPoint(int Id, int IdCannale, bool upsert = false);
        /// <summary>
        /// Update/Create point
        /// </summary>
        /// <param name="pointDto"></param>
        /// <returns></returns>
        Task<bool> UpsertPointPatchAsync(PointDto pointDto);
        /// <summary>
        /// Update/Create point for method patch
        /// </summary>
        /// <param name="pointDto"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        Task<bool> UpsertPointAsync(PointDto pointDto, bool patch = false);
    }
}
