using Core.Enums;
using Core.Models;
using Core.Pricing;
using Core.Pricing.Abstractions;
using Core.Strategies;
using FluentAssertions;

namespace Tests
{
    public class PriceCalculatorTests
    {
        private readonly PriceCalculator _sut;

        public PriceCalculatorTests()
        {
            var strategies = new IPriceStrategy[]
            {
            new SubcompactPriceStrategy(),
            new StationWagonPriceStrategy(),
            new TruckPriceStrategy()
            };

            var resolver = new PriceStrategyResolver(strategies);
            _sut = new PriceCalculator(resolver);
        }

        [Fact]
        public void Calculate_ShouldUseCorrectStrategy_AndRound()
        {
            var request = new PriceCalculationRequest
            {
                Type = CarTypeEnum.Truck,
                BaseDayPrice = 100m,
                BaseKmPrice = 2m,
                TotalDays = 1.11111m,
                TotalKm = 10m
            };

            var result = _sut.CalculatePrice(request);

            result.Should().Be(Math.Round(result, 4));
        }
    }
}
