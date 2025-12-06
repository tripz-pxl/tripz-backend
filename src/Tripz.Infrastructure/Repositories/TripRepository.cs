using Microsoft.EntityFrameworkCore;
using Tripz.AppLogic.Queries;
using Tripz.AppLogic.Services;
using Tripz.Domain.Entities;
using Tripz.Infrastructure.Data;

namespace Tripz.Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TripzDbContext _context;

        public TripRepository(TripzDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetTripsAsync(GetTripsQuery query)
        {
            var tripsQuery = _context.Trips
                .Include(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.EmployeeId))
            {
                if (int.TryParse(query.EmployeeId, out int userId))
                {
                    tripsQuery = tripsQuery.Where(t => t.UserId == userId);
                }
            }

            if (query.TransportType.HasValue)
            {
                tripsQuery = tripsQuery.Where(t => (int)t.TransportType == query.TransportType.Value);
            }

            if (query.Month.HasValue && query.Year.HasValue)
            {
                tripsQuery = tripsQuery.Where(t =>
                    t.DepartureDate.Month == query.Month.Value &&
                    t.DepartureDate.Year == query.Year.Value);
            }

            return await tripsQuery
                .OrderByDescending(t => t.SubmittedAt)
                .ToListAsync();
        }

        public async Task<Trip> CreateTripAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            
            // Load the User navigation property after creation
            await _context.Entry(trip).Reference(t => t.User).LoadAsync();
            
            return trip;
        }

        public async Task<Trip?> GetTripByIdAsync(Guid id)
        {
            return await _context.Trips
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateTripAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Trip>> GetTripsForEmployeeAsync(int employeeId)
        {
            return await _context.Trips
                .Include(t => t.User)
                .Where(t => t.UserId == employeeId)
                .OrderByDescending(t => t.SubmittedAt)
                .ToListAsync();
        }

    }
}