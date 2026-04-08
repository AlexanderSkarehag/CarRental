using Core.Entities;

namespace Core.Specifications.Rentals
{
    internal class ActiveRentalsSpecification : BaseSpecification<RentalEntity>
    {
        public ActiveRentalsSpecification(bool asNoTracking = true) : base(r => r.RentalFinish == null ||r.RentalFinish == DateTimeOffset.MinValue)
        {
            if (asNoTracking)
                ApplyNoTracking();
        }
    }
}
