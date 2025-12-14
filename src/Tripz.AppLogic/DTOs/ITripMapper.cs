using Tripz.Domain.Entities;

namespace Tripz.AppLogic.DTOs
{
    public interface ITripMapper
    {
        TripDto ToDto(Trip trip);
    }
}
