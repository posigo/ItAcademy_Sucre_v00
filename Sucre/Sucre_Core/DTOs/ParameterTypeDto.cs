namespace Sucre_Core.DTOs
{
    /// <summary>
    /// Типы физических параметров
    /// </summary>    
    public class ParameterTypeDto : IBaseEntity<int>
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Mnemo { get; set; } = string.Empty;
        
        public string UnitMeas { get; set; } = string.Empty;
    }
}
