using System.Linq.Dynamic.Core;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class ODataExtensions
    {
        public static IQueryable AppendQuery(this IQueryable queryable, string filter = default, int? skip = null, int? top = null, string orderBy = default)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                queryable = queryable.Where(filter);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                queryable = queryable.OrderBy(orderBy);
            }
            if (skip != null)
            {
                queryable = queryable.Skip(skip.Value);
            }
            if (top != null)
            {
                queryable = queryable.Take(top.Value);
            }

            return queryable;
        }
    }
}
