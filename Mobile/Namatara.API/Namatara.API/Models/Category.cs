namespace Namatara.API.Models
{
    public class Category : _AuditEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}