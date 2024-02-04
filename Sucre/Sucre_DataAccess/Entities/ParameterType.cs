using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Типы физических параметров
    /// </summary>
    [Table("ParameterTypes")]
    public class ParameterType : IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(5)]
        public string Mnemo { get; set; } = string.Empty;
        [MaxLength(8)]
        public string UnitMeas { get; set; } = string.Empty;

        public virtual ICollection<Canal> Canals { get; set; }
    }
}
