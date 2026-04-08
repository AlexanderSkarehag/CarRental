using Core.Enums;
using Core.Models;

namespace Core.Pricing.Abstractions
{
    public interface IPriceStrategy
    {
        CarTypeEnum Type { get; }
        decimal Calculate(PriceCalculationRequest req);
    }
}
