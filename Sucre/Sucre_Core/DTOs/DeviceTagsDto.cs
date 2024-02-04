namespace Sucre_Core.DTOs
{
    public class DeviceTagsDto
    {
        public DeviceDto DeviceDto { get; set; }
        public IEnumerable<DeviceTagDto> Tags {  get; set; } 
    }
}
