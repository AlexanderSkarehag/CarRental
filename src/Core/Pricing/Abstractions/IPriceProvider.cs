using Core.Enums;

namespace Core.Pricing.Abstractions
{
    public interface IPriceProvider
    {
        decimal GetBaseDayPrice(CarTypeEnum type);
        decimal? GetBaseKmPrice(CarTypeEnum type);
    }
}
