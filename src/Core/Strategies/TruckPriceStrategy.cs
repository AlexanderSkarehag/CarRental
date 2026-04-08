using Core.Enums;
using Core.Models;
using Core.Pricing.Abstractions;

namespace Core.Strategies
{
    public sealed class TruckPriceStrategy : IPriceStrategy
    {
        public CarTypeEnum Type => CarTypeEnum.Truck;

        public decimal Calculate(PriceCalculationRequest r)
            => r.BaseDayPrice * r.TotalDays * 1.5m
             + (r.BaseKmPrice ?? 0) * (r.TotalKm ?? 0) * 1.5m;
    }
}
