using Core.Enums;
using Core.Models;
using Core.Pricing.Abstractions;

namespace Core.Strategies
{
    public sealed class StationWagonPriceStrategy : IPriceStrategy
    {
        public CarTypeEnum Type => CarTypeEnum.StationWagon;

        public decimal Calculate(PriceCalculationRequest r)
            => r.BaseDayPrice * r.TotalDays * 1.3m
             + (r.BaseKmPrice ?? 0) * (r.TotalKm ?? 0);
    }
}
