using Core.Entities;
using Core.Specifications;

namespace Tests.Specifications.Models
{
    internal class TestCarSpecification : BaseSpecification<CarEntity>
    {
        public TestCarSpecification(decimal minMilage) : base(c => c.CurrentMilageInKm > minMilage)
        {

        }
    }
}
