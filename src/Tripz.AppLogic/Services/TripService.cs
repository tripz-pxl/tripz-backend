using Tripz.AppLogic.Commands;
using Tripz.AppLogic.DTOs;
using Tripz.AppLogic.Queries;
using Tripz.Domain.Enums;
using Tripz.Domain.State;

namespace Tripz.AppLogic.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripMapper _tripMapper;
        private readonly ITripFactory _tripFactory;

        public TripService(ITripRepository tripRepository, ITripMapper tripMapper, ITripFactory tripFactory)
        {
            _tripRepository = tripRepository;
            _tripMapper = tripMapper;
            _tripFactory = tripFactory;
        }

        public async Task<IEnumerable<TripDto>> GetTripsAsync(GetTripsQuery query)
        {
            var trips = await _tripRepository.GetTripsAsync(query);

            // Ensure filters are applied even if repository ignored them
            var filtered = trips.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.EmployeeId))
            {
                filtered = filtered.Where(t =>
                    string.Equals(t.User.Id.ToString(), query.EmployeeId, StringComparison.OrdinalIgnoreCase));
            }

            if (query.TransportType.HasValue)
            {
                filtered = filtered.Where(t => (int)t.TransportType == query.TransportType.Value);
            }

            if (query.Month.HasValue)
            {
                filtered = filtered.Where(t => t.DepartureDate.Month == query.Month.Value);
            }

            if (query.Year.HasValue)
            {
                filtered = filtered.Where(t => t.DepartureDate.Year == query.Year.Value);
            }

            return filtered.Select(t => _tripMapper.ToDto(t));
        }

        public async Task<TripDto?> GetTripByIdAsync(Guid id)
        {
            var trip = await _tripRepository.GetTripByIdAsync(id);

            if (trip == null)
                return null;

            return _tripMapper.ToDto(trip);
        }

        public async Task<TripDto> CreateTripAsync(CreateTripCommand command)
        {
            var trip = _tripFactory.CreateFromCommand(command);
            var createdTrip = await _tripRepository.CreateTripAsync(trip);
            return _tripMapper.ToDto(createdTrip);
        }

        public async Task<TripDto?> ApproveTripAsync(ApproveTripCommand command)
        {
            var trip = await _tripRepository.GetTripByIdAsync(command.TripId);

            if (trip == null)
                return null;

            if (command.Status == TripStatus.Approved)
            {
                trip.Approve();
            }
            else if (command.Status == TripStatus.Rejected)
            {
                trip.Reject(command.Reason);
            }
            else
            {
                throw new InvalidOperationException("Invalid status for approval action.");
            }

            await _tripRepository.UpdateTripAsync(trip);

            return _tripMapper.ToDto(trip);
        }

        public async Task<IEnumerable<TripDto>> GetTripsForEmployeeAsync(int employeeId)
        {
            var trips = await _tripRepository.GetTripsForEmployeeAsync(employeeId);

            return trips.Select(t => _tripMapper.ToDto(t));
        }
    }
}