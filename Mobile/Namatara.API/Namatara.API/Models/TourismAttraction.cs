namespace Namatara.API.Models
{
    public class TourismAttraction : _AuditEntity
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; } = 0;
        public string Name { get; set; } = string.Empty;  // Name of the tourism attraction
        public string? Description { get; set; }  // Description of the attraction
        public string Location { get; set; } = string.Empty;  // Location of the attraction
        public string OpeningHours { get; set; } = string.Empty;  // Opening hours of the attraction
        public decimal Rating { get; set; } = 0;  // Store average rating here
        public string? ImageUrl { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<TourismAttractionRating> Ratings { get; set; } = [];
    }
}