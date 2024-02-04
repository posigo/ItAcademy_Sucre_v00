namespace Sucre_WebApi.Models
{
    /// <summary>
    /// Точки учета
    /// </summary>    
    public class PointApiM 
    {        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string EnergyName { get; set; }
        public string CexName { get; set;}
        public string? ServiceStaff { get; set; } = string.Empty; 

    }
}
