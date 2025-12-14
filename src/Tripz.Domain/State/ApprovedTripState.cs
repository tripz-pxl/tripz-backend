using Tripz.Domain.Entities;
using Tripz.Domain.Enums;

namespace Tripz.Domain.State;

internal class ApprovedTripState : ITripState
{
    public Trip ApproveTrip(Trip trip)
    {
        throw new InvalidOperationException("Trip is already approved.");
    }

    public Trip RejectTrip(Trip trip, string? reason)
    {
        throw new InvalidOperationException("An approved trip cannot be rejected.");
    }
}