using Tripz.AppLogic.Commands;
using Tripz.Domain.Entities;
using Tripz.Domain.Enums;

namespace Tripz.AppLogic.Services
{
    public class TripFactory : ITripFactory
    {
        public Trip CreateFromCommand(CreateTripCommand command)
        {
            return new Trip
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                TransportType = command.TransportType,
                DepartureDate = command.DepartureDate,
                ReturnDate = command.ReturnDate,
                Destination = command.Destination,
                Distance = command.Distance,
                Purpose = command.Purpose,
                EstimatedCost = command.EstimatedCost,
                Status = TripStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            };
        }
    }
}
