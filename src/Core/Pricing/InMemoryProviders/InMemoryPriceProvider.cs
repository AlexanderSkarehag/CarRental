using Core.Enums;
using Core.Pricing.Abstractions;

namespace Core.Pricing.InMemoryProviders
{
    public sealed class InMemoryPriceProvider : IPriceProvider
    {
        private static readonly Dictionary<CarTypeEnum, (decimal day, decimal? km)> _prices = new()
        {
            [CarTypeEnum.Subcompact] = (150m, null),
            [CarTypeEnum.StationWagon] = (200m, 2.0m),
            [CarTypeEnum.Truck] = (300m, 3.5m)
        };

        public decimal GetBaseDayPrice(CarTypeEnum type)
            => _prices.TryGetValue(type, out var price)
                ? price.day
                : throw new NotImplementedException($"No pricing for {type}");

        public decimal? GetBaseKmPrice(CarTypeEnum type)
            => _prices.TryGetValue(type, out var price)
                ? price.km
                : throw new NotImplementedException($"No pricing for {type}");
    }
}
