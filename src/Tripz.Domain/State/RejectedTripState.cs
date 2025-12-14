using Tripz.Domain.Entities;

namespace Tripz.Domain.State;

internal class RejectedTripState : ITripState
{
    public Trip ApproveTrip(Trip trip)
    {
        throw new InvalidOperationException("A rejected trip cannot be approved.");
    }

    public Trip RejectTrip(Trip trip, string? reason)
    {
        throw new InvalidOperationException("Trip is already rejected.");
    }
}

