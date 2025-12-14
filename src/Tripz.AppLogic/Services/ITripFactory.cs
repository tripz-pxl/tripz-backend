using Tripz.AppLogic.Commands;
using Tripz.Domain.Entities;

namespace Tripz.AppLogic.Services
{
    public interface ITripFactory
    {
        Trip CreateFromCommand(CreateTripCommand command);
    }
}
