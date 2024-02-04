using Sucre_Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sucre_DataAccess.Entities
{
    public class RefreshToken : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime GenerateAt { get; set; }
        public DateTime ExpiringAt { get; set;}
        public string AssociatedDeviceName { get; set; }
        public Guid AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

    }
}
