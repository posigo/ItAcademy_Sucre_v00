using System.ComponentModel.DataAnnotations;

namespace Sucre_Models
{
    /// <summary>
    /// каналы учёта
    /// </summary>    
    public class CanalM
    {        
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int ParameterTypeId { get; set; }        
        public string? ParameterTypeName { get; set; }
        [Required]
        public bool Reader { get; set; }
        /// <summary>
        /// Тип читаемого параметра
        /// 0-устройство
        /// 1-внешний файл
        /// 2-ручной ввод
        /// </summary>        
        [Required]
        [Range(0,2)]
        public int SourceType { get; set; }
        [Required]
        public bool AsPazEin { get; set; } = false;
        public bool AsPazEmpty { get; set; } = false;
        public bool Hour { get; set; } = true;
        public bool Day { get; set; } = false;
        public bool Month { get; set; } = false;

        public CanalM()
        {            
            
        }
    }
}
