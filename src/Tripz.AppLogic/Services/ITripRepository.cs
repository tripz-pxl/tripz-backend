using Tripz.AppLogic.Queries;
using Tripz.Domain.Entities;

namespace Tripz.AppLogic.Services
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetTripsAsync(GetTripsQuery query);
        Task<IEnumerable<Trip>> GetTripsForEmployeeAsync(int employeeId);
        Task<Trip> CreateTripAsync(Trip trip);
        Task<Trip?> GetTripByIdAsync(Guid id);
        Task UpdateTripAsync(Trip trip);
    }
}