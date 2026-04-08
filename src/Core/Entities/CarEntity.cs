using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Core.Entities
{
    public class CarEntity : BaseEntity<Guid>, IAggregateRoot<Guid>
    {
        public CarEntity()
        {
            
        }
        public CarEntity(Car model)
        {
            Id = Guid.NewGuid();
            LicensePlate = model.LicensePlate;
            CurrentMilageInKm = model.CurrentMilageInKm;
            CarType = model.CarType;
        }
        public string LicensePlate { get; set; }
        public double CurrentMilageInKm { get; set; } = 0;
        public CarTypeEnum CarType { get; set; }
        public string PartitionKey { get; set; } = "_static";
        public string Discriminator { get; set; } = nameof(CarEntity);
    }
}
