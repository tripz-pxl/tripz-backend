using Tripz.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Tripz.AppLogic.DTOs
{
    [Mapper]
    public partial class TripMapper : ITripMapper
    {
		public partial TripDto ToDto(Trip trip);
	}
}
