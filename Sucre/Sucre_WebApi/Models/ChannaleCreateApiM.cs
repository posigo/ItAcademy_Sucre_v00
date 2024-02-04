using System.ComponentModel.DataAnnotations;

namespace Sucre_WebApi.Models
{
    /// <summary>
    /// каналы учёта
    /// </summary>    
    public class ChannaleCreateApiM
    {
        [Required]
        public string Name { get; set; } = string.Empty;        
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int ParameterTypeId { get; set; }
        public bool Reader { get; set; } = true;
        /// <summary>
        /// Тип читаемого параметра
        /// 0-устройство
        /// 1-внешний файл
        /// 2-ручной ввод
        /// </summary>        
        [Range(0, 2)]
        public int SourceType { get; set; } = 0;        
        public bool AsPazEin { get; set; } = false;
        
    }
}
