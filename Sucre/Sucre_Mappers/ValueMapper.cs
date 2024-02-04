using Riok.Mapperly.Abstractions;
using Sucre_Core.DTOs;
using Sucre_DataAccess.Entities;
using Sucre_Models;

namespace Sucre_Mappers
{
    [Mapper]
    public partial class ValueMapper
    {   
        public partial ValueHourDto ValueHourToValueHourDto(ValueHour valueHour);
        public partial ValueHour ValueHourDtoToValueHour(ValueHourDto valueHourDto);
        public partial ValueDayDto ValueDayToValueDayDto(ValueDay valueDay);
        public partial ValueDay ValueDayDtoToValueDay(ValueDayDto valueDayDto);

    }
}
