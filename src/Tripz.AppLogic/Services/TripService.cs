using System;
using System.Linq;
using Tripz.AppLogic.Commands;
using Tripz.AppLogic.DTOs;
using Tripz.AppLogic.Queries;
using Tripz.Domain.Entities;
using Tripz.Domain.Enums;
using Tripz.Domain.State;

namespace Tripz.AppLogic.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
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

            return filtered.Select(t => new TripDto
            {
                Id = t.Id,
                EmployeeId = t.User.Id.ToString(),
                EmployeeName = t.User.Nickname,
                TransportType = t.TransportType.ToString(),
                DepartureDate = t.DepartureDate,
                ReturnDate = t.ReturnDate,
                Destination = t.Destination,
                EstimatedCost = t.EstimatedCost,
                Status = t.Status.ToString(),
                SubmittedAt = t.SubmittedAt
            });
        }

        public async Task<TripDto?> GetTripByIdAsync(Guid id)
        {
            var trip = await _tripRepository.GetTripByIdAsync(id);

            if (trip == null)
                return null;

            return new TripDto
            {
                Id = trip.Id,
                EmployeeId = trip.User.Id.ToString(),
                EmployeeName = trip.User.Nickname,
                TransportType = trip.TransportType.ToString(),
                DepartureDate = trip.DepartureDate,
                ReturnDate = trip.ReturnDate,
                Destination = trip.Destination,
                EstimatedCost = trip.EstimatedCost,
                Status = trip.Status.ToString(),
                SubmittedAt = trip.SubmittedAt
            };
        }

        public async Task<TripDto> CreateTripAsync(CreateTripCommand command)
        {
            var trip = new Trip
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

            var createdTrip = await _tripRepository.CreateTripAsync(trip);

            return new TripDto
            {
                Id = createdTrip.Id,
                EmployeeId = createdTrip.User.Id.ToString(),
                EmployeeName = createdTrip.User.Nickname,
                TransportType = createdTrip.TransportType.ToString(),
                DepartureDate = createdTrip.DepartureDate,
                ReturnDate = createdTrip.ReturnDate,
                Destination = createdTrip.Destination,
                EstimatedCost = createdTrip.EstimatedCost,
                Status = createdTrip.Status.ToString(),
                SubmittedAt = createdTrip.SubmittedAt
            };
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

            return new TripDto
            {
                Id = trip.Id,
                EmployeeId = trip.User.Id.ToString(),
                EmployeeName = trip.User.Nickname,
                TransportType = trip.TransportType.ToString(),
                DepartureDate = trip.DepartureDate,
                ReturnDate = trip.ReturnDate,
                Destination = trip.Destination,
                EstimatedCost = trip.EstimatedCost,
                Status = trip.Status.ToString(),
                Reason = trip.Reason,
                SubmittedAt = trip.SubmittedAt
            };
        }

        public async Task<IEnumerable<TripDto>> GetTripsForEmployeeAsync(int employeeId)
        {
            var trips = await _tripRepository.GetTripsForEmployeeAsync(employeeId);

            return trips.Select(t => new TripDto
            {
                Id = t.Id,
                EmployeeId = t.User.Id.ToString(),
                EmployeeName = t.User.Nickname,
                TransportType = t.TransportType.ToString(),
                DepartureDate = t.DepartureDate,
                ReturnDate = t.ReturnDate,
                Destination = t.Destination,
                EstimatedCost = t.EstimatedCost,
                Status = t.Status.ToString(),
                SubmittedAt = t.SubmittedAt
            });
        }
    }
}