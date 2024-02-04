using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IAsPazService
    {
        /// <summary>
        /// Check and remove AsPaz for channale with Id
        /// </summary>
        /// <param name="Id">Id channale</param>
        /// <returns>0 -- error, 1 -- no AsPaz, 2 -- remove AsPaz</returns>
        Task<int> CheckAndDelByChanaleIdAsync(int Id);
        /// <summary>
        /// Remove entity AsPaz
        /// </summary>
        /// <param name="asPazDto">Entity AsPaz</param>
        /// <returns>TRUE -- Remove Ok, FALSE -- Remove failed</returns>
        Task<bool> DeleteAsPazAsync(AsPazDto asPazDto);
        /// <summary>
        /// Remove AsPaz for by Id entity
        /// </summary>
        /// <param name="Id">Id entity AsPaz</param>
        /// <returns>TRUE -- Remove Ok, FALSE -- Remove failed</returns>
        Task<bool> DeleteAsPazByIdAsync(int Id);
        /// <summary>
        /// Get AsPaz for by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>object AsPaz</returns>
        Task<AsPazDto> GetAsPazByIdAsync(int Id);
        /// <summary>
        /// Get values AsPaz for by Id
        /// </summary>
        /// <param name="Id">Id AsPaz</param>
        /// <returns></returns>
        Task<AsPazChannaleDto> GetAsPazChannaleByIdAsync(int Id);
        /// <summary>
        /// Get values AsPaz for channale with Id
        /// </summary>
        /// <param name="IdCanale">Id channale</param>
        /// <returns></returns>
        Task<AsPazChannaleDto> GetAsPazChannaleByIdCanAsync(int IdCanale);
        /// <summary>
        /// Get list AsPaz
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AsPazChannaleDto>> GetListAsPasChannaleAsync();
        /// <summary>
        /// Create/update values AsPaz
        /// </summary>
        /// <param name="asPazDto">object AsPaz</param>
        /// <returns></returns>
        Task<bool> UpsertAsPazAsync(AsPazDto asPazDto);
    }
}