using Tripz.Domain.Enums;

namespace Tripz.AppLogic.Commands
{
    public class ApproveTripCommand
    {
        public Guid TripId { get; set; }
        public TripStatus Status { get; set; }
        public string? Reason { get; set; }
    }
}

