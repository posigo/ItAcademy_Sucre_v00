namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class PointDto 
    {        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int EnergyId { get; set; }
        public int CexId { get; set;}
        public string? ServiceStaff { get; set; } = string.Empty; 

    }
}
