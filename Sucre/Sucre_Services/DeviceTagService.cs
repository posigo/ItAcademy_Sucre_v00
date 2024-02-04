using MediatR;
using Microsoft.Extensions.Configuration;
using Sucre_Core.DTOs;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.CQS.Commands;
using Sucre_DataAccess.CQS.Queries;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;
using Sucre_Mappers;
using Sucre_Services.Interfaces;

namespace Sucre_Services
{
    public class DeviceTagService: IDeviceTagService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ISucreUnitOfWork _sucreUnitOfWork;
        private readonly DeviceMapper _deviceMapper;
        private readonly CanalMapper _canalMapper;
        private readonly ParameterTypeMapper _parameterTypeMapper;
        private readonly PointMapper _pointMapper;        

        public DeviceTagService(
            IConfiguration configuration,
            IMediator mediator,
            ISucreUnitOfWork sucreUnitOfWork,
            DeviceMapper deviceMapper,
            CanalMapper canalMapper,
            ParameterTypeMapper parameterTypeMapper,
            PointMapper pointMapper)
        {
            _configuration = configuration;
            _mediator = mediator;
            _sucreUnitOfWork = sucreUnitOfWork;
            _deviceMapper = deviceMapper;
            _canalMapper = canalMapper;
            _parameterTypeMapper = parameterTypeMapper;
            _pointMapper = pointMapper;
        }

        public async Task<bool> CreateDevice(DeviceDto deviceDto)
        {
            try
            {
                var command = new AddDeviceCommand()
                {
                    Device = _deviceMapper.DeviceDtoToDevice(deviceDto)
                };
                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> CreateTag(DeviceTagDto deviceTagDto)
        {
            try
            {
                if (deviceTagDto == null) { return false; }
                DeviceTag deviceTag = _deviceMapper.DeviceTagDtoToDeviceTag(deviceTagDto);
                AddTagCommand command = new AddTagCommand()
                {
                    Tag = _deviceMapper.DeviceTagDtoToDeviceTag(deviceTagDto)
                };
                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"!!!->Error, DeviceTagService->{nameof(PatchDevice)}: {ex.Message}");
            }

            return false;
        }


        public async Task<bool> DeleteDeviceById(int Id)
        {
            try
            {
                if (Id == 0) return false;
                DeleteDeviceCommand command = new DeleteDeviceCommand() { Id = Id };
                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->Error, DeviceTagService->{nameof(DeleteDeviceById)}: {ex.Message}");
            }
            return false;
        }

        public async Task<List<DeviceDto>> GetDevices()
        {
            try
            {
                GetDevicesQuery query = new GetDevicesQuery();
                var devicesDb = await _mediator.Send(query);
                if (devicesDb == null || devicesDb.Count == 0) return null;

                var devicesDto = devicesDb
                    .Select(device => _deviceMapper.DeviceToDeviceDto(device));
                return devicesDto.ToList();


            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->DevicesService->{nameof(GetDevices)}: {ex.Message}");
            }
            return null;

        }

        public async Task<DeviceTagsDto> GetTagsByDevice(int id)
        {
            try
            {
                DeviceTagsDto deviceTagsDto = new DeviceTagsDto();
                GetDeviceByIdQuery queryd = new GetDeviceByIdQuery() { Id = id};
                var device = await _mediator.Send(queryd);
                deviceTagsDto.DeviceDto = _deviceMapper.DeviceToDeviceDto(device);

                GetDeviceTagsQuery queryt = new GetDeviceTagsQuery() { DeviceId = id };
                var tags = await _mediator.Send(queryt);
                deviceTagsDto.Tags = (tags
                    .Select(tag => _deviceMapper.DeviceTagToDeviceTagDto(tag)));
                return deviceTagsDto;

            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->DevicesService->{nameof(GetDevices)}: {ex.Message}");
            }
            return null;

        }

        public async Task<List<DeviceTagDto>> GetTags()
        {
            try
            {
                GetDeviceTagsQuery query = new GetDeviceTagsQuery();
                var deviceTagsDb = await _mediator.Send(query);
                if (deviceTagsDb == null || deviceTagsDb.Count == 0) return null;

                var deviceTagsDto = deviceTagsDb
                    .Select(tag => _deviceMapper.DeviceTagToDeviceTagDto(tag));
                return deviceTagsDto.ToList();


            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->DevicesService->{nameof(GetDevices)}: {ex.Message}");
            }
            return null;

        }

        public async Task<DeviceDto> GetDeviceDtoById(int id)
        {
            try
            {
                GetDeviceByIdQuery query = new GetDeviceByIdQuery() { Id = id };
                var device = await _mediator.Send(query);
                if (device == null) return null;
                var deviceDto = _deviceMapper.DeviceToDeviceDto(device);
                return deviceDto;

            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"*->DevicesService->{nameof(GetDevices)}: {ex.Message}");
            }
            return null;
        }
                
        public async Task<bool> PatchDevice(DeviceDto deviceDto)
        {
            try
            {
                if (deviceDto == null) { return false; }
                Device device = _deviceMapper.DeviceDtoToDevice(deviceDto); 
                PatchDeviceCommand command = new PatchDeviceCommand()
                { 
                    Id = device.Id,
                    PatchDtos = new List<PatchDto>()
                    {
                        new() {PropertyName = nameof(device.Name), PropertyValue = device.Name },
                        new() {PropertyName = nameof(device.Connection), PropertyValue = device.Connection }
                    }
                };

                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"!!!->Error, DeviceTagService->{nameof(PatchDevice)}: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> RemoveTag(DeviceTagDto deviceTagDto)
        {
            try
            {
                if (deviceTagDto == null) { return false; }
                DeviceTag deviceTag = _deviceMapper.DeviceTagDtoToDeviceTag(deviceTagDto);
                DeleteTagCommand command = new DeleteTagCommand()
                {
                    Tag = _deviceMapper.DeviceTagDtoToDeviceTag(deviceTagDto)
                };
                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"!!!->Error, DeviceTagService->{nameof(RemoveTag)}: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> UpdateDeviceTag(DeviceTagDto deviceTagDto)
        {
            try
            {
                if (deviceTagDto == null) { return false; }
                DeviceTag deviceTag = _deviceMapper.DeviceTagDtoToDeviceTag(deviceTagDto);
                UpdateTagCommand command = new UpdateTagCommand()
                {
                    DeviceId = deviceTag.Id,
                    ChannaleId = deviceTag.ChannaleId,
                    Environment = deviceTagDto.Enviroment,
                    ParameterCode = deviceTagDto.ParameterCode                    
                };
                await _mediator.Send(command);
                return true;
            }
            catch (Exception ex)
            {
                LoggerExternal.LoggerEx.Error($"!!!->Error, DeviceTagService->{nameof(PatchDevice)}: {ex.Message}");
            }

            return false;
        }

        
    }
}
