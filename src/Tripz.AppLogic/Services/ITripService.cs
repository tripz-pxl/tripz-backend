using Tripz.AppLogic.Commands;
using Tripz.AppLogic.DTOs;
using Tripz.AppLogic.Queries;

namespace Tripz.AppLogic.Services
{
    public interface ITripService
    {
        Task<IEnumerable<TripDto>> GetTripsAsync(GetTripsQuery query);
        Task<IEnumerable<TripDto>> GetTripsForEmployeeAsync(int employeeId);
        Task<TripDto> CreateTripAsync(CreateTripCommand command);
        Task<TripDto?> GetTripByIdAsync(Guid id);
        Task<TripDto?> ApproveTripAsync(ApproveTripCommand command);
    }
}