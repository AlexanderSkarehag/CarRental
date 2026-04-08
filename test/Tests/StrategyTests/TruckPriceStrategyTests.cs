using Core.Enums;
using Core.Models;
using Core.Strategies;
using FluentAssertions;

namespace Tests.StrategyTests
{
    public class TruckPriceStrategyTests
    {
        private readonly TruckPriceStrategy _sut = new();

        [Fact]
        public void Calculate_ShouldApplyTruckPricingRules()
        {
            // Arrange
            var request = new PriceCalculationRequest
            {
                Type = CarTypeEnum.Truck,
                BaseDayPrice = 100m,
                BaseKmPrice = 2m,
                TotalDays = 2m,
                TotalKm = 10m
            };

            // Act
            var result = _sut.Calculate(request);

            // Assert
            // (100 * 2 * 1.5) + (2 * 10 * 1.5) = 300 + 30 = 330
            result.Should().Be(330m);
        }
    }
}
