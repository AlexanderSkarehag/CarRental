using System;
using System.Collections.Generic;
using System.Text;
using Core.Enums;
using Core.Models;
using Core.Strategies;
using FluentAssertions;

namespace Tests.StrategyTests
{
    public class StationWagonPriceStrategyTests
    {
        private readonly StationWagonPriceStrategy _sut = new();

        [Fact]
        public void Calculate_ShouldApplyStationWagonPricing()
        {
            var request = new PriceCalculationRequest
            {
                Type = CarTypeEnum.StationWagon,
                BaseDayPrice = 100m,
                BaseKmPrice = 2m,
                TotalDays = 2m,
                TotalKm = 10m
            };

            var result = _sut.Calculate(request);

            // (100 * 2 * 1.3) + (2 * 10) = 260 + 20 = 280
            result.Should().Be(280m);
        }
    }
}
