namespace Sucre_Core.DTOs
{
    public class AppUserDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int GroupNumber { get; set; } = 99;
    }
}
