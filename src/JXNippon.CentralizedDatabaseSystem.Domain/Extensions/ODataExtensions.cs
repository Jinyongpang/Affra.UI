using System.Linq.Dynamic.Core;
using Microsoft.OData.Client;
using Microsoft.OData.Extensions.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    public static class ODataExtensions
    {
        public static async Task<QueryOperationResponse<T>> ToQueryOperationResponseAsync<T>(this IQueryable queryable)
        {
            return (await queryable.ExecuteAsync<T>()) as QueryOperationResponse<T>;
        }

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
