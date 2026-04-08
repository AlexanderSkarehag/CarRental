using Core.Enums;

namespace Core.Consts
{
    public static class PriceList
    {
        public static decimal GetBaseDayPrice(CarTypeEnum type) => type switch
        {
            CarTypeEnum.Subcompact => 150m,
            CarTypeEnum.StationWagon => 200m,
            CarTypeEnum.Truck => 300m,
            _ => throw new NotImplementedException()
        };
        public static decimal? GetBaseKmPrice(CarTypeEnum type) => type switch
        {
            CarTypeEnum.Subcompact => null,
            CarTypeEnum.StationWagon => 2.0m,
            CarTypeEnum.Truck => 3.5m,
            _ => throw new NotImplementedException()
        };
    }
}
