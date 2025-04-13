namespace Namatara.API.Models;

public class TourismAttractionBookmark : _AuditEntity
{
    public Guid Id { get; set; }  // Unique ID for each rating entry
    public Guid TourismAttractionId { get; set; }  // Reference to TourismAttraction
    public Guid UserId { get; set; }  

    // Navigation properties
    public virtual TourismAttraction? TourismAttraction { get; set; } 
    public virtual User? User { get; set; }
   
} 

public class TourismAttractionBookmarkRequest
{
    public Guid TourismAttractionId { get; set; }  // Reference to TourismAttraction
}