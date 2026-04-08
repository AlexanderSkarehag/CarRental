using Core.Interfaces;
using Core.Models;

namespace Core.Entities
{
    public class RentalEntity : BaseEntity<Guid>, IAggregateRoot<Guid>
    {
        public RentalEntity()
        {
        }
        public RentalEntity(Rental model)
        {
            RentalStart = model.RentalStart;
            RentalFinish = model.RentalFinish;
            CarStartMilageInKm = model.CarStartMilageInKm;
            CarFinishMilageInKm = model.CarFinishMilageInKm;
            TotalCost = model.TotalCost;
        }
        public required IdValuePair<Guid> CarIdLicense { get; set; }
        public required string PersonalIdentification { get; set; } //personal number in Sweden/letters and numbers combo in other countries.
        public DateTimeOffset RentalStart { get; set; }
        public DateTimeOffset? RentalFinish { get; set; }
        public decimal CarStartMilageInKm { get; set; }
        public decimal? CarFinishMilageInKm { get; set; }
        public decimal TotalMilageInKm => CarFinishMilageInKm ?? CarStartMilageInKm - CarStartMilageInKm;
        public decimal? TotalCost { get; set; }

    }
}
