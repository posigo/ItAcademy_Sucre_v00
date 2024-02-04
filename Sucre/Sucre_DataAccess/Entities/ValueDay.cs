using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// значения по суткам
    /// </summary>
    [Table("ValuesDay")]
    public class ValueDay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Value { get; set; }
        public bool Changed { get; set; }

        [ForeignKey("Id")]
        public Canal Canal { get; set; }
    }
}
