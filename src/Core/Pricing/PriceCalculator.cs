using Core.Models;

namespace Core.Pricing
{
    public sealed class PriceCalculator
    {
        private readonly PriceStrategyResolver _resolver;

        public PriceCalculator(PriceStrategyResolver resolver)
        {
            _resolver = resolver;
        }
        public decimal CalculatePrice(PriceCalculationRequest req)
        {
            var strategy = _resolver.Get(req.Type);
            var total = strategy.Calculate(req);

            return Math.Round(total, 4);
        }
    }
    
}
