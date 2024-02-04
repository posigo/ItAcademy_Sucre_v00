namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class ChannalePointsDto
    {        
        public CanalDto ChannaleDto { get; set; }
        public  IEnumerable<PointDto> PointsDto { get; set; }

    }

    
}
