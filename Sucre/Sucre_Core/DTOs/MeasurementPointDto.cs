using Sucre_DataAccess.Entities;

namespace Sucre_Core.DTOs
{
    public class MeasurementPointDto
    {
        public PointTableDto PointTableDto { get; set; } = new PointTableDto();

        public List<CannaleFullDto> ChannalesFullDto { get; set; } = 
            new List<CannaleFullDto> { new CannaleFullDto() };

        public List<ValueHourDto> ValuesHourDto { get; set; } = 
            new List<ValueHourDto> { new ValueHourDto() };
    }
}
