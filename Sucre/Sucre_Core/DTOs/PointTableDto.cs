namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class PointTableDto
    {        
        public PointDto PointDto { get; set; }
        public string? EnergyName { get; set; }       
        public string? CexName { get; set; }

    }

    
}
