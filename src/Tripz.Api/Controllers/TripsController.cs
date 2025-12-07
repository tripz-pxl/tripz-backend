using Microsoft.AspNetCore.Mvc;
using Tripz.Api.Models;
using Tripz.AppLogic.Commands;
using Tripz.AppLogic.Queries;
using Tripz.AppLogic.Services;
using Tripz.Domain.Enums;

namespace Tripz.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        /// <summary>
        /// Get all submitted trips with optional filtering and pagination.
        /// Allows managers to filter by employee, transport type, or date (month/year).
        /// </summary>
        /// <param name="request">Filter and pagination parameters</param>
        /// <returns>Paginated list of trips with metadata</returns>
        /// <response code="200">Returns the paginated list of trips</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [Authorize(Roles = "Manager")] // TODO: Uncomment when authentication is implemented
        public async Task<IActionResult> GetTrips([FromQuery] GetTripsRequest request)
        {
            // With [ApiController], invalid model state returns 400 automatically.
            var query = new GetTripsQuery
            {
                EmployeeId = request.EmployeeId,
                TransportType = request.TransportType,
                Month = request.Month,
                Year = request.Year
            };

            var trips = (await _tripService.GetTripsAsync(query)).ToList();
            var totalCount = trips.Count();

            var paginatedTrips = trips
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = new
            {
                data = paginatedTrips,
                pagination = new
                {
                    page = request.Page,
                    pageSize = request.PageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
                }
            };

            return Ok(result);
        }

        /// <summary>
        /// Get a specific trip by ID.
        /// </summary>
        /// <param name="id">The trip ID</param>
        /// <returns>The trip details</returns>
        /// <response code="200">Trip found</response>
        /// <response code="404">Trip not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTripById(Guid id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);

            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        /// <summary>
        /// Register a new trip for an employee.
        /// Allows employees to submit a trip with date, destination, distance, transport type, purpose, and cost.
        /// </summary>
        /// <param name="request">Trip details for registration</param>
        /// <returns>The created trip details</returns>
        /// <response code="201">Trip successfully registered</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
        {
            var command = new CreateTripCommand
            {
                UserId = request.UserId,
                TransportType = (TransportType)request.TransportType,
                DepartureDate = request.DepartureDate,
                ReturnDate = request.ReturnDate,
                Destination = request.Destination,
                Distance = request.Distance,
                Purpose = request.Purpose,
                EstimatedCost = request.EstimatedCost
            };

            var createdTrip = await _tripService.CreateTripAsync(command);

            return CreatedAtAction(nameof(GetTripById), new { id = createdTrip.Id }, createdTrip);
        }

        /// <summary>
        /// Approve or reject a trip reimbursement request.
        /// Allows managers to approve (status=2) or reject (status=3) a submitted trip.
        /// </summary>
        /// <param name="id">The trip ID</param>
        /// <param name="request">Approval decision and optional reason</param>
        /// <returns>The updated trip details</returns>
        /// <response code="200">Trip successfully approved or rejected</response>
        /// <response code="400">Invalid status or trip cannot be approved</response>
        /// <response code="404">Trip not found</response>
        [HttpPatch("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Manager")] // TODO: Uncomment when authentication is implemented
        public async Task<IActionResult> ApproveTrip(Guid id, [FromBody] ApproveTripRequest request)
        {
            var command = new ApproveTripCommand
            {
                TripId = id,
                Status = (TripStatus)request.Status,
                Reason = request.Reason
            };

            try
            {
                var updatedTrip = await _tripService.ApproveTripAsync(command);

                if (updatedTrip == null)
                    return NotFound();

                return Ok(updatedTrip);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("employee")]
        public async Task<IActionResult> GetTripsForEmployee([FromQuery] int userId)
        {
            var trips = await _tripService.GetTripsForEmployeeAsync(userId);
            return Ok(trips);
        }
    }
}