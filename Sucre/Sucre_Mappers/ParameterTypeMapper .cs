using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class ParameterTypeMapper
    {
        public partial ParameterTypeDto ParameterToParameterDto(ParameterType parameterType);
        public partial ParameterType ParameterDtoToParameter(ParameterTypeDto parameterTypeDto);
        public partial ParameterTypeM ParameterDtoToModel(ParameterTypeDto parameterTypeDto);
        public partial ParameterTypeDto ModelToParameterDto(ParameterTypeM parameterTypeM);
    }
}
