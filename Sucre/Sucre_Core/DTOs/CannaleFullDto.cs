namespace Sucre_Core.DTOs
{
    public class CannaleFullDto
    {
        public CanalDto CannaleDto { get; set; }
        public ParameterTypeDto ParameterTypeDto { get; set; }
        public AsPazDto? AsPazDto { get; set; } = null;
    }
}
