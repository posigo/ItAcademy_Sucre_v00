namespace Sucre_Core.DTOs
{
    public class MeasurementPointHDto
    {
        /// <summary>
        /// точка учёта
        /// </summary>
        public PointTableDto PointTableDto { get; set; } = new PointTableDto();
        /// <summary>
        /// список каналов в точке учёта
        /// </summary>
        public List<CannaleFullDto> ChannalesFullDto { get; set; } = 
            new List<CannaleFullDto> { new CannaleFullDto() };
        /// <summary>
        /// значения каналов, используемых в точке учёта
        /// </summary>
        public List<ValueDayDto> ValuesDayDto { get; set; } = 
            new List<ValueDayDto> { new ValueDayDto() };
    }
}
