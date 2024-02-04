namespace Sucre_Core.DTOs
{
    /// <summary>
    /// точки учёта короткое название
    /// </summary>    
    public class PointShortNameDto
    {   
        public int Id { get; set; }        
        public string Name { get; set; } = string.Empty;                    
        public int? EnergyId { get; set; }
        public string? EnergyName { get; set; }
        public int? CexId { get; set; }
        public string? CexName { get; set; }

    }
}
