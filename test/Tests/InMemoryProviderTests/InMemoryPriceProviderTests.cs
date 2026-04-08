using Core.Enums;
using Core.Pricing.InMemoryProviders;
using FluentAssertions;

namespace Tests.InMemoryProviderTests
{
    public class InMemoryPriceProviderTests
    {
        private readonly InMemoryPriceProvider _sut = new();

        [Fact]
        public void GetBaseDayPrice_ShouldReturnCorrectValue()
        {
            var result = _sut.GetBaseDayPrice(CarTypeEnum.Truck);

            result.Should().Be(300m);
        }

        [Fact]
        public void GetBaseKmPrice_ShouldReturnCorrectValue()
        {
            var result = _sut.GetBaseKmPrice(CarTypeEnum.StationWagon);

            result.Should().Be(2.0m);
        }

        [Fact]
        public void GetBaseKmPrice_Subcompact_ShouldReturnNull()
        {
            var result = _sut.GetBaseKmPrice(CarTypeEnum.Subcompact);

            result.Should().BeNull();
        }
    }
}
