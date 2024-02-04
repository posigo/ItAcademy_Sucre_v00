namespace Sucre_Core.DTOs
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public DateTime GenerateAt { get; set; }
        public DateTime ExpiringAt { get; set;}
        public string AssociatedDeviceName { get; set; }
        public Guid AppUserId { get; set; }

        public bool? IsValid
        {
            get
            {
                if (GenerateAt < ExpiringAt &&
                    DateTime.UtcNow<ExpiringAt)
                    return true;
                return false;   
            }
        }

    }
}
