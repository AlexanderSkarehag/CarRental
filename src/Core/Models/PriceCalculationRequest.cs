using Core.Enums;

namespace Core.Models
{
    public record PriceCalculationRequest
    {
        public CarTypeEnum Type { get; init; }
        public decimal BaseDayPrice { get; init; }
        public decimal TotalDays { get; init; }
        public decimal? BaseKmPrice { get; init; }
        public decimal? TotalKm { get; init; }
    }
}
