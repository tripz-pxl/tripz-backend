using Tripz.Domain.Enums;
using Tripz.Domain.State;

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

        private ITripState? _state;

        public ITripState State
        {
            get
            {
                if (_state == null)
                    InitializeStateFromStatus();
                return _state!;
            }
            private set => _state = value;
        }

        internal void SetState(ITripState state) => State = state;

        public Trip Approve()
        {
            return State.ApproveTrip(this);
        }

        public Trip Reject(string? reason)
        {
            return State.RejectTrip(this, reason);
        }

        private void InitializeStateFromStatus()
        {
            switch (Status)
            {
                case TripStatus.Submitted:
                    _state = new SubmittedTripState();
                    break;
                case TripStatus.Approved:
                    _state = new ApprovedTripState();
                    break;
                case TripStatus.Rejected:
                    _state = new RejectedTripState();
                    break;
                case TripStatus.Completed:
                    _state = new CompletedTripState();
                    break;
                default:
                    _state = new SubmittedTripState();
                    break;
            }
        }
    }
}