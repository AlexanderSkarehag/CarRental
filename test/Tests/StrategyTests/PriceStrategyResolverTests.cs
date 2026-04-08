using Core.Enums;
using Core.Pricing;
using Core.Pricing.Abstractions;
using Core.Strategies;
using FluentAssertions;

namespace Tests.StrategyTests
{
    public class PriceStrategyResolverTests
    {
        private readonly PriceStrategyResolver _sut;

        public PriceStrategyResolverTests()
        {
            var strategies = new IPriceStrategy[]
            {
            new SubcompactPriceStrategy(),
            new StationWagonPriceStrategy(),
            new TruckPriceStrategy()
            };

            _sut = new PriceStrategyResolver(strategies);
        }

        [Fact]
        public void Get_ShouldReturnCorrectStrategy()
        {
            var strategy = _sut.Get(CarTypeEnum.Truck);

            strategy.Should().BeOfType<TruckPriceStrategy>();
        }

        [Fact]
        public void Get_UnknownType_ShouldThrow()
        {
            var act = () => _sut.Get(CarTypeEnum.Unknown);

            act.Should().Throw<NotImplementedException>();
        }
    }
}
