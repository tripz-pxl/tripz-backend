using Tripz.Domain.Entities;
using Tripz.Domain.Enums;

namespace Tripz.AppLogic.Services
{
    public static class ReimbursementCalculator
    {
        public static decimal Calculate(Trip trip)
        {
            switch (trip.TransportType)
            {
                case TransportType.Bike:
                    return trip.Distance * 0.25m;
                case TransportType.Car:
                    return trip.Distance * 0.40m;
                case TransportType.Train:
                case TransportType.Plane:
                case TransportType.Bus:
                    return trip.EstimatedCost;
                case TransportType.Other:
                default:
                    return 0;
            }
        }
    }
}
