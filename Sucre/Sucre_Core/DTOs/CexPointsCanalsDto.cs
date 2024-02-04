namespace Sucre_Core.DTOs
{
    public class CexPointsCanalsDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<PointCanalesDto> PointsCanalesDto { get; set; } = new List<PointCanalesDto>();
    }
}
