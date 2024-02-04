using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// типы энергии
    /// </summary>
    [Table("Enegies")]
    public class Energy: IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(15)]
        public string EnergyName { get; set; } = string.Empty;

        public virtual ICollection<Point> Points { get; set; }

        public Energy()
        {
            this.Points = new HashSet<Point>();
        }
    }
}
