using Tripz.Domain.Enums;

namespace Tripz.Domain.Entities
{
    public class Trip
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public TransportType TransportType { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Destination { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public TripStatus Status { get; set; }
        public string? Reason { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}