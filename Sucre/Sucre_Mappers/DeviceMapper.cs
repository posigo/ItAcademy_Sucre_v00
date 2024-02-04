using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class DeviceMapper
    {
        public partial DeviceDto DeviceToDeviceDto(Device device);
        public partial Device DeviceDtoToDevice(DeviceDto deviceDto);
        public partial DeviceTagDto DeviceTagToDeviceTagDto(DeviceTag deviceTag);
        public partial DeviceTag DeviceTagDtoToDeviceTag(DeviceTagDto deviceTagDto);
        public partial TagM DeviceTagDtoToTagM(DeviceTagDto deviceTagDto);
        public partial DeviceTagDto TagMToDeviceTagDto(TagM tagM);
    }
}
