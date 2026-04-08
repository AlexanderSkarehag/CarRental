using Core.Entities;

namespace Core.Models
{
    public record Rental
    {
        public Rental()
        {
        }
        public Rental(RentalEntity entity)
        {
            Id = entity.Id;
            CarIdLicense = entity.CarIdLicense;
            PersonalIdentification = entity.PersonalIdentification;
            RentalStart = entity.RentalStart;
            RentalFinish = entity.RentalFinish;
            CarStartMilageInKm = entity.CarStartMilageInKm;
            CarFinishMilageInKm = entity.CarFinishMilageInKm;
        }
        public Guid Id { get; set; }
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
