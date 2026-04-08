using Core.Enums;
using Core.Models;
using Core.Strategies;
using FluentAssertions;

namespace Tests.StrategyTests
{
    public class SubcompactPriceStrategyTests
    {
        private readonly SubcompactPriceStrategy _sut = new();

        [Fact]
        public void Calculate_ShouldOnlyUseDayPrice()
        {
            var request = new PriceCalculationRequest
            {
                Type = CarTypeEnum.Subcompact,
                BaseDayPrice = 100m,
                TotalDays = 3m
            };

            var result = _sut.Calculate(request);

            result.Should().Be(300m);
        }
    }
}
