namespace Namatara.API.Models
{
    public class TicketBooking : _AuditEntity
    { 
        public Guid Id { get; set; }
        public Guid TourismAttractionId { get; set; }
        public Guid UserId { get; set; }

        public string Code { get; set; }
 
        public int NumberOfTickets { get; set; }
        public decimal InputPrice { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingExpiredDate { get; set; }

        public virtual TourismAttraction? TourismAttraction { get; set; }
        public virtual User? User { get; set; }

        // Method to generate random code
       
    }

    public class TicketBookingRequest
    {
        public Guid TourismAttractionId { get; set; } 
        public int NumberOfTickets { get; set; }
        public int InputPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingExpiredDate { get; set; }
    }
}