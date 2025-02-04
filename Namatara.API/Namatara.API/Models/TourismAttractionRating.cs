namespace Namatara.API.Models
{
    public class TourismAttractionRating
    {
        public Guid Id { get; set; }  // Unique ID for each rating entry
        public Guid TourismAttractionId { get; set; }  // Reference to TourismAttraction
        public Guid UserId { get; set; }  // Reference to the User who gave the rating
        public decimal Rating { get; set; }  // Rating value (e.g., between 1 and 5)
        public string? Review { get; set; }  // Optional review/feedback from the user

        // Navigation properties
        public virtual TourismAttraction? TourismAttraction { get; set; }

        public virtual User? User { get; set; }
    }

    public class TourismAttractionUserRating
    {
        public Guid TourismAttractionId { get; set; }
        public decimal Rating { get; set; }
        public string? Review { get; set; }
    }
}