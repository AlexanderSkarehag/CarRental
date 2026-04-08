using Core.Entities;

namespace Core.Specifications.Rentals
{
    internal class GetActiveRentalByIdSpecification : BaseSpecification<RentalEntity>
    {
        public GetActiveRentalByIdSpecification(Guid carId)
            : base(c => c.CarIdLicense.Id == carId && c.CarFinishMilageInKm == null)
        {
            
        }
    }
}
