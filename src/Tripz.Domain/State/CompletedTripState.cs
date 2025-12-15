using Tripz.Domain.Entities;

namespace Tripz.Domain.State;

internal class CompletedTripState : ITripState
{
    public Trip ApproveTrip(Trip trip)
    {
        throw new InvalidOperationException("A completed trip cannot be approved.");
    }

    public Trip RejectTrip(Trip trip, string? reason)
    {
        throw new InvalidOperationException("A completed trip cannot be rejected.");
    }
}

