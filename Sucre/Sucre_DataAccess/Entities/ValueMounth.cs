using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// значения по месяцам
    /// </summary>
    [Table("ValuesMounth")]
    public class ValueMounth
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Value { get; set; }
        public bool Changed { get; set; }
        public bool PlanFact { get; set; }

        [ForeignKey("Id")]
        public Canal Canal { get; set; }
    }
}
