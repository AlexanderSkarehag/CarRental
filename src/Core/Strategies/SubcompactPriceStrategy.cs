using Core.Enums;
using Core.Models;
using Core.Pricing.Abstractions;

namespace Core.Strategies
{
    public sealed class SubcompactPriceStrategy : IPriceStrategy
    {
        public CarTypeEnum Type => CarTypeEnum.Subcompact;

        public decimal Calculate(PriceCalculationRequest r)
            => r.BaseDayPrice * r.TotalDays;
    }
}
