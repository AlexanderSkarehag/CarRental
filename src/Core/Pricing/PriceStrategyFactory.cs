using Core.Enums;
using Core.Pricing.Abstractions;
using Core.Strategies;

namespace Core.Pricing
{
    public static class PriceStrategyFactory
    {
        public static IPriceStrategy Get(CarTypeEnum type) => type switch
        {
            CarTypeEnum.Subcompact => new SubcompactPriceStrategy(),
            CarTypeEnum.StationWagon => new StationWagonPriceStrategy(),
            CarTypeEnum.Truck => new TruckPriceStrategy(),
            _ => throw new NotImplementedException()
        };
    }
}
