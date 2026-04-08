using Core.Entities;

namespace Core.Specifications.Cars
{
    internal class CarsByIdsSpecification : BaseSpecification<CarEntity>
    {
        public CarsByIdsSpecification(List<Guid> ids, bool asNoTracking = true) : base(c => !ids.Contains(c.Id))
        {
            if (asNoTracking)
                ApplyNoTracking();
        }
    }
}
