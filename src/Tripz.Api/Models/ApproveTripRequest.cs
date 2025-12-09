using System.ComponentModel.DataAnnotations;

namespace Tripz.Api.Models
{
    public class ApproveTripRequest
    {
        [Required]
        [Range(2, 3, ErrorMessage = "Status must be 2 (Approved) or 3 (Rejected)")]
        public int Status { get; set; }

        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string? Reason { get; set; }
    }
}