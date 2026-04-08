using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public static class EfSpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(
            IQueryable<T> inputQuery,
            ISpecification<T> spec
            )
        {
            var query = inputQuery;

            //Criteria
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            //Includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            //Ordering
            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            //Tracking
            if (spec.IsNoTracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
