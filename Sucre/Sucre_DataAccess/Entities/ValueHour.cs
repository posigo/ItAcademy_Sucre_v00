using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// Часовое значение
    /// </summary>
    [Table("ValuesHour")]
    public class ValueHour
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public decimal Value { get; set; }
        public bool Changed { get; set; }

        [ForeignKey("Id")]
        public Canal Canal { get; set; }
    }
}

