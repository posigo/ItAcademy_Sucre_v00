using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sucre_Core;

namespace Sucre_DataAccess.Entities
{
    /// <summary>
    /// каналы учёта
    /// </summary>
    [Table("Canals")]
    public class Canal : IBaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int ParameterTypeId { get; set; }
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
        public bool Hour { get; set; } = true;
        public bool Day {  get; set; } = false;
        public bool Month { get; set; } = false;
        public virtual AsPaz? AsPaz { get; set; }
        [ForeignKey("ParameterTypeId")]
        public virtual ParameterType ParameterType { get; set; }
        public virtual ICollection<Point> Points { get; set; }
        public virtual ICollection<ValueHour> ValueHours { get; set; }
        public virtual ICollection<ValueDay> ValueDays { get; set; }
        public virtual ICollection<ValueMounth> ValueMounths { get; set; }
        //public virtual ICollection<ReportDetail> ReportDetails { get; set; } 

        public Canal()
        {
            Reader = true;
            this.Points = new HashSet<Point>();
            this.ValueHours = new HashSet<ValueHour>();
            this.ValueDays = new HashSet<ValueDay>();
            this.ValueMounths = new HashSet<ValueMounth>();
            //this.ReportDetails = new HashSet<ReportDetail>();
        }
    }
}
