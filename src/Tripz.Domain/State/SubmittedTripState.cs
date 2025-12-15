using Tripz.Domain.Entities;
using Tripz.Domain.Enums;

namespace Tripz.Domain.State;

internal class SubmittedTripState : ITripState
{
    public Trip ApproveTrip(Trip trip)
    {
        trip.Status = TripStatus.Approved;
        trip.Reason = null;
        trip.SetState(new ApprovedTripState());
        return trip;
    }

    public Trip RejectTrip(Trip trip, string? reason)
    {
        trip.Status = TripStatus.Rejected;
        trip.Reason = reason;
        trip.SetState(new RejectedTripState());
        return trip;
    }
}

