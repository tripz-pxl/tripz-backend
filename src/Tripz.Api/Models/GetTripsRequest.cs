using System.ComponentModel.DataAnnotations;
using Tripz.Domain.Enums;

namespace Tripz.Api.Models
{
    public class GetTripsRequest
    {
        public string? EmployeeId { get; set; }

        [EnumDataType(typeof(TransportType), ErrorMessage = "Transport type must be a valid value")]
        public int? TransportType { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int? Month { get; set; }

        public int? Year { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
    }
}