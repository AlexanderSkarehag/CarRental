using Core.Entities;
using Core.Enums;

namespace Core.Models
{
    public record Car
    {
        public Car() { }

        public Car(CarEntity entity)
        {
            Id = entity.Id;
            LicensePlate = entity.LicensePlate;
            CurrentMilageInKm = entity.CurrentMilageInKm;
            CarType = entity.CarType;
        }
        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public decimal CurrentMilageInKm { get; set; } = 0;
        public CarTypeEnum  CarType { get; set; }
    }
}
