namespace Sucre_Core.DTOs
{
    public class ChannalePointsFullDto
    {
        public CanalDto CannaleDto { get; set; }
        public ParameterTypeDto ParameterTypeDto { get; set; }
        public AsPazDto? AsPazDto { get; set; } = null;
        public IEnumerable<PointTableDto> PointsTableDto { get; set; }
    }
}
