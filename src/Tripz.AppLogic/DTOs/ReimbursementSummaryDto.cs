namespace Tripz.AppLogic.DTOs
{
    public class ReimbursementSummaryDto
    {
        public decimal TotalReimbursement { get; set; }
        public Dictionary<string, decimal> ByMonth { get; set; } = [];
        public Dictionary<string, decimal> ByTransportType { get; set; } = [];
    }
}
