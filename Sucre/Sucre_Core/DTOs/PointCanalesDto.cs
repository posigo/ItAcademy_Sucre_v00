namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class PointCanalesDto
    {        
        public PointDto PointDto { get; set; }
        public IEnumerable<CanalDto> CanalesDto { get; set; }

    }

    
}
