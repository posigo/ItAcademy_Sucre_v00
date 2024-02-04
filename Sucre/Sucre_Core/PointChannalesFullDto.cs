namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class PointChannalesFullDto
    {        
        public PointTableDto PointTableDto { get; set; }
        public IEnumerable<CanalShortNameDto> ChannalesShortDto { get; set; }

    }

    
}
