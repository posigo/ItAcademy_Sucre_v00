using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Описание местоположения точки
    /// </summary>
    [Table("Cexs")]
    public class Cex : IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Управление
        /// </summary>
        [MaxLength(35)]
        public string? Management { get; set; }
        /// <summary>
        /// цех
        /// </summary>
        [MaxLength(35)]
        public string? CexName { get; set; }
        /// <summary>
        /// участок
        /// </summary>
        [MaxLength(50)]
        public string? Area { get; set; }
        /// <summary>
        /// устанорвка
        /// </summary>
        [MaxLength(50)]
        public string? Device { get; set; }
        /// <summary>
        /// локация
        /// </summary>
        [MaxLength(70)]
        public string? Location { get; set; }

        public virtual ICollection<Point> Points { get; set; }
    }
}
