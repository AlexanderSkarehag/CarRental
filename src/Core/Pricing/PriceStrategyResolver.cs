using Core.Enums;
using Core.Pricing.Abstractions;

namespace Core.Pricing
{
    public sealed class PriceStrategyResolver
    {
        private readonly IReadOnlyDictionary<CarTypeEnum, IPriceStrategy> _strategies;

        public PriceStrategyResolver(IEnumerable<IPriceStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(s => s.Type);
        }

        public IPriceStrategy Get(CarTypeEnum type)
        {
            if (_strategies.TryGetValue(type, out var strategy))
            return strategy;

            throw new NotImplementedException($"No pricing strategy for {type}");
        }
    }
}
