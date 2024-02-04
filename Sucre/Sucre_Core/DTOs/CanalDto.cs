using System.ComponentModel.DataAnnotations;

namespace Sucre_Core.DTOs
{
    /// <summary>
    /// каналы учёта
    /// </summary>    
    public class CanalDto
    {   
        public int Id { get; set; }        
        public string Name { get; set; } = string.Empty;        
        public string? Description { get; set; } = string.Empty;        
        public int ParameterTypeId { get; set; }                
        public bool Reader { get; set; }
        /// <summary>
        /// Тип читаемого параметра
        /// 0-устройство
        /// 1-внешний файл
        /// 2-ручной ввод
        /// </summary>        
        [Range(0,2)]
        public int SourceType { get; set; }        
        public bool AsPazEin { get; set; } = false;
        public bool Hour { get; set; } = true;
        public bool Day { get; set; } = false;
        public bool Month { get; set; } = false;
        public CanalDto()
        {
            this.Reader = true;            
            this.SourceType = 0;
            
        }
    }
}
