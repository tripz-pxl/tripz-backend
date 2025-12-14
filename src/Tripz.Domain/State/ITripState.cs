using Tripz.Domain.Entities;

namespace Tripz.Domain.State;

public interface ITripState
{
    Trip ApproveTrip(Trip trip);
    Trip RejectTrip(Trip trip, string? reason);
}