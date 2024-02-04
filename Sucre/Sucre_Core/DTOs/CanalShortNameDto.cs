namespace Sucre_Core.DTOs
{
    /// <summary>
    /// каналы учёта короткое название
    /// </summary>    
    public class CanalShortNameDto
    {   
        public int Id { get; set; }        
        public string Name { get; set; } = string.Empty;                    
        public int? ParameterTypeId { get; set; }
        public string? ParameterTypeName { get; set; }
        
    }
}
