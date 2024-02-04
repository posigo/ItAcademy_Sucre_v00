using Sucre_Core.DTOs;

namespace Sucre_Services.Interfaces
{
    public interface IDeviceTagService
    {
        /// <summary>
        /// Создать устройство
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        Task<bool> CreateDevice(DeviceDto deviceDto);
        /// <summary>
        /// Слинковать тег
        /// </summary>
        /// <param name="deviceTagDto"></param>
        /// <returns></returns>
        Task<bool> CreateTag(DeviceTagDto deviceTagDto);
        /// <summary>
        /// Удалить устройство
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> DeleteDeviceById(int Id);
        /// <summary>
        /// Получить список устройств
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceDto>> GetDevices();
        /// <summary>
        /// Устройство
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceDto> GetDeviceDtoById(int id);
        /// <summary>
        /// Получить список тегов устройства
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeviceTagsDto> GetTagsByDevice(int id);
        /// <summary>
        /// Список всех тегов
        /// </summary>
        /// <returns></returns>
        Task<List<DeviceTagDto>> GetTags();
        /// <summary>
        /// Обновить данные об устройстве
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <returns></returns>
        Task<bool> PatchDevice(DeviceDto deviceDto);
        /// <summary>
        /// Remove link tag
        /// </summary>
        /// <param name="deviceTagDto"></param>
        /// <returns></returns>
Task<bool> RemoveTag(DeviceTagDto deviceTagDto);
        /// <summary>
        /// Обновление тега устройства
        /// </summary>
        /// <param name="deviceTagDto"></param>
        /// <returns></returns>
        Task<bool> UpdateDeviceTag(DeviceTagDto deviceTagDto);
    //    /// <summary>
    //    /// Create channale (mediator)
    //    /// </summary>
    //    /// <param name="canalDto"></param>
    //    /// <returns></returns>
    //    Task<bool> CreateChannaleAsync(CanalDto canalDto);
    //    /// <summary>
    //    /// Remove channale for by Id
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    Task<bool> DeleteChannaleByIdAsync(int Id);
    //    /// <summary>
    //    /// Get channale for by Id
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    Task<CanalDto> GetCannaleByIdAsync(int Id);
    //    /// <summary>
    //    /// Get full info a channale for by Id
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    Task<CannaleFullDto> GetCannaleByIdFullAsync(int Id);
    //    /// <summary>
    //    /// Get a channale and assigned metering points
    //    /// </summary>
    //    /// <param name="Id">Id channale</param>       
    //    /// <returns></returns>
    //    Task<ChannalePointsDto> GetChannalePointesAsync(int Id);
    //    Task<ChannalePointsFullDto> GetChannalePointsFullAsync(int id);
    //    /// <summary>
    //    /// Get list channales
    //    /// </summary>
    //    /// <returns></returns>
    //    Task<IEnumerable<CanalDto>> GetListCannalesAsync();
    //    /// <summary>
    //    /// Get list channales with full info a channale
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <returns></returns>
    //    Task<IEnumerable<CannaleFullDto>> GelListCannalesFullAsync(int? Id);
    //    /// <summary>
    //    /// Get list chanales for Id with set list id
    //    /// </summary>
    //    /// <param name="listIds">list Id</param>
    //    /// <param name="tEqual">flag equal</param>
    //    /// <param name="paramName"></param>
    //    /// <returns></returns>
    //    List<CanalShortNameDto> GetListCanalesForId(
    //        List<int> listIds = null,
    //        bool tEqual = false,
    //        bool paramName = false);
    //    /// <summary>
    //    /// Update channale for method Patch
    //    /// </summary>
    //    /// <param name="canalDto"></param>
    //    /// <returns></returns>
    //    Task<bool> UpsertCanalAsync(CanalDto canalDto);
    //    /// <summary>
    //    /// Create/update channale
    //    /// </summary>
    //    /// <param name="Id"></param>
    //    /// <param name="IdPoint"></param>
    //    /// <param name="upsert">user method Patch</param>
    //    /// <returns></returns>
    //    Task<bool> UpsertPointToCanal(int Id, int IdPoint, bool upsert = false);
    //    Task Z_HangFire(int type, params string[] values);

        //    Task<bool> ReadValueChannaleFromDevice(int id, string baseUri);

        //    Task<bool> ReadValuesChannalesFromDevice(string baseUri);
    }
}
