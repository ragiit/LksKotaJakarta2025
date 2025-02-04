namespace Namatara.API.Models
{
    public class TicketBooking : _AuditEntity
    {
        private string _code = string.Empty;

        public Guid Id { get; set; }
        public Guid TourismAttractionId { get; set; }
        public Guid UserId { get; set; }

        public string Code
        {
            get
            {
                // Generate the code if it's not set yet
                if (string.IsNullOrEmpty(_code))
                {
                    _code = GenerateBookingCode(12);  // Generates a random 12-character code for booking
                }
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        public int NumberOfPersons { get; set; }
        public int NumberOfTickets { get; set; }
        public int InputPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime BookingExpiredDate { get; set; }

        public virtual TourismAttraction? TourismAttraction { get; set; }
        public virtual User? User { get; set; }

        // Method to generate random code
        private static string GenerateBookingCode(int length = 6)
        {
            const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = validCharacters[random.Next(validCharacters.Length)];
            }

            return new string(code);
        }
    }
}