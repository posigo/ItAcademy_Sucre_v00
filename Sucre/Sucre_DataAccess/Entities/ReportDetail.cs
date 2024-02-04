using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    [Table("ReportDetails")]
    public class ReportDetail
    {
        public int ReportId { get; set; }
        public int PointId { get; set; }
        public int CanalId { get; set; }

        public virtual Report Report { get; set; }
        public virtual Point Point { get; set; }
        public virtual Canal Canal { get; set; }
    }
}
