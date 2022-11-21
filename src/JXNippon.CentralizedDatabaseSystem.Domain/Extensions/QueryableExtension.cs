using System.Collections;
using System.Linq.Dynamic.Core;
using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Extensions
{
    /// <summary>
    /// Class QueryableExtension.
    /// </summary>
    public static class QueryableExtension
    {
        private static Func<FilterDescriptor, bool> canFilter = (c) => c.FilterOperator == FilterOperator.IsNull
                || c.FilterOperator == FilterOperator.IsNotNull
                || c.FilterOperator == FilterOperator.IsEmpty
                || c.FilterOperator == FilterOperator.IsNotEmpty
                || !(c.FilterValue == null || c.FilterValue as string == string.Empty);
        public static IQueryable<T> AppendQueryWithFilterDescriptor<T>(this IQueryable<T> queryable, IEnumerable<FilterDescriptor> filterDescriptors = default, int? skip = null, int? top = null, string orderBy = default)
        {
            Type type = typeof(T);
            return AppendQueryWithFilterDescriptor(queryable, type, filterDescriptors, skip, top, orderBy) as IQueryable<T>;
        }
        public static IQueryable AppendQueryWithFilterDescriptor(this IQueryable queryable, Type type, IEnumerable<FilterDescriptor> filterDescriptors = default, int? skip = null, int? top = null, string orderBy = default)
        {
            if (filterDescriptors is not null)
            {
                queryable = queryable.Where(filterDescriptors, type);
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
        /// <summary>
        /// The filter operators
        /// </summary>
        internal static readonly IDictionary<string, string> FilterOperators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"ne", "!="},
            {"lt", "<"},
            {"le", "<="},
            {"gt", ">"},
            {"ge", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"DoesNotContain", "Contains"}
        };

        /// <summary>
        /// The linq filter operators
        /// </summary>
        internal static readonly IDictionary<FilterOperator, string> LinqFilterOperators = new Dictionary<FilterOperator, string>
        {
            {FilterOperator.Equals, "="},
            {FilterOperator.NotEquals, "!="},
            {FilterOperator.LessThan, "<"},
            {FilterOperator.LessThanOrEquals, "<="},
            {FilterOperator.GreaterThan, ">"},
            {FilterOperator.GreaterThanOrEquals, ">="},
            {FilterOperator.StartsWith, "StartsWith"},
            {FilterOperator.EndsWith, "EndsWith"},
            {FilterOperator.Contains, "Contains"},
            {FilterOperator.DoesNotContain, "DoesNotContain"},
            {FilterOperator.IsNull, "=="},
            {FilterOperator.IsEmpty, "=="},
            {FilterOperator.IsNotNull, "!="},
            {FilterOperator.IsNotEmpty, "!="}
        };

        /// <summary>
        /// The o data filter operators
        /// </summary>
        internal static readonly IDictionary<FilterOperator, string> ODataFilterOperators = new Dictionary<FilterOperator, string>
        {
            {FilterOperator.Equals, "eq"},
            {FilterOperator.NotEquals, "ne"},
            {FilterOperator.LessThan, "lt"},
            {FilterOperator.LessThanOrEquals, "le"},
            {FilterOperator.GreaterThan, "gt"},
            {FilterOperator.GreaterThanOrEquals, "ge"},
            {FilterOperator.StartsWith, "startswith"},
            {FilterOperator.EndsWith, "endswith"},
            {FilterOperator.Contains, "contains"},
            {FilterOperator.DoesNotContain, "DoesNotContain"},
            {FilterOperator.IsNull, "eq"},
            {FilterOperator.IsEmpty, "eq"},
            {FilterOperator.IsNotNull, "ne"},
            {FilterOperator.IsNotEmpty, "ne"}
        };

        /// <summary>
        /// Converts to filterstring.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns">The columns.</param>
        /// <returns>System.String.</returns>
        public static string ToFilterString(this IEnumerable<FilterDescriptor> filterDescriptors, Type type)
        {
            if (filterDescriptors.Where(canFilter).Any())
            {
                var firstLogicalFilterOperator = filterDescriptors.FirstOrDefault()?.LogicalFilterOperator;
                var firstBooleanOperator = firstLogicalFilterOperator == LogicalFilterOperator.And ? "and" : "or";

                var whereList = new List<string>();
                foreach (var filter in filterDescriptors.Where(canFilter))
                {
                    var value = filter.FilterValue;
                    var secondValue = filter.SecondFilterValue;

                    if ((value is not null && !string.IsNullOrEmpty(value.ToString()))
                        || filter.FilterOperator == FilterOperator.IsNotNull
                        || filter.FilterOperator == FilterOperator.IsNull
                        || filter.FilterOperator == FilterOperator.IsEmpty
                        || filter.FilterOperator == FilterOperator.IsNotEmpty)
                    {
                        var linqOperator = LinqFilterOperators[filter.FilterOperator];
                        if (linqOperator == null)
                        {
                            linqOperator = "==";
                        }

                        var booleanOperator = filter.LogicalFilterOperator == LogicalFilterOperator.And ? "and" : "or";

                        if ((secondValue is not null && !string.IsNullOrEmpty(secondValue.ToString()))
                           || filter.SecondFilterOperator == FilterOperator.IsNotNull
                           || filter.SecondFilterOperator == FilterOperator.IsNull
                           || filter.SecondFilterOperator == FilterOperator.IsEmpty
                           || filter.SecondFilterOperator == FilterOperator.IsNotEmpty) 
                        {
                            whereList.Add($"({GetColumnFilter(type, filter)} {booleanOperator} {GetColumnFilter(type, filter, true)})");
                        }
                        else
                        {
                            whereList.Add(GetColumnFilter(type, filter));
                        }
                    }
                }

                return string.Join($" {firstBooleanOperator} ", whereList.Where(i => !string.IsNullOrEmpty(i)));
            }

            return "";
        }
        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }

        /// <summary>
        /// Gets the column filter.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <param name="second">if set to <c>true</c> [second].</param>
        /// <returns>System.String.</returns>
        private static string GetColumnFilter(Type objectType, FilterDescriptor filter, bool second = false)
        {
            var property = filter.Property;
            Type propertyType = objectType.GetProperty(property).PropertyType;
            Type type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            object filterValue = !second ? filter.FilterValue : filter.SecondFilterValue;

            if (property.IndexOf(".") != -1)
            {
                property = $"({property})";
            }

            var filterOperator = !second ? filter.FilterOperator : filter.SecondFilterOperator;

            var linqOperator = LinqFilterOperators[filterOperator];
            if (linqOperator == null)
            {
                linqOperator = "==";
            }

            if (type == typeof(string))
            {
                string stringValue = filterValue as string;
                string filterCaseSensitivityOperator = ".ToLower()";

                if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.Contains)
                {
                    return $@"{property}{filterCaseSensitivityOperator}.Contains(""{stringValue}""{filterCaseSensitivityOperator})";
                }
                else if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.DoesNotContain)
                {
                    return $@"!{property}{filterCaseSensitivityOperator}.Contains(""{stringValue}""{filterCaseSensitivityOperator})";
                }
                else if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.StartsWith)
                {
                    return $@"{property}{filterCaseSensitivityOperator}.StartsWith(""{stringValue}""{filterCaseSensitivityOperator})";
                }
                else if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.EndsWith)
                {
                    return $@"{property}{filterCaseSensitivityOperator}.EndsWith(""{stringValue}""{filterCaseSensitivityOperator})";
                }
                else if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.Equals)
                {
                    return $@"{property}{filterCaseSensitivityOperator} == ""{stringValue}""{filterCaseSensitivityOperator}";
                }
                else if (!string.IsNullOrEmpty(stringValue) && filterOperator == FilterOperator.NotEquals)
                {
                    return $@"{property}{filterCaseSensitivityOperator} != ""{stringValue}""{filterCaseSensitivityOperator}";
                }
                else if (filterOperator == FilterOperator.IsNull)
                {
                    return $@"{property} == null";
                }
                else if (filterOperator == FilterOperator.IsEmpty)
                {
                    return $@"{property} == """"";
                }
                else if (filterOperator == FilterOperator.IsNotEmpty)
                {
                    return $@"{property} != """"";
                }
                else if (filterOperator == FilterOperator.IsNotNull)
                {
                    return $@"{property} != null";
                }
            }
            else if (type.IsEnum || type.IsNullableEnum())
            {
                if (filterOperator == FilterOperator.IsNull || filterOperator == FilterOperator.IsNotNull)
                {
                    return $"{property} {linqOperator} null";
                }
                else if (filterOperator == FilterOperator.IsEmpty || filterOperator == FilterOperator.IsNotEmpty)
                {
                    return $@"{property} {linqOperator} """"";
                }
                else
                {
                    var enumResult = Enum.Parse(type, filterValue.ToString());
                    return $"{property}.ToString() {linqOperator} \"{enumResult}\"";
                }
            }
            else if (PropertyAccess.IsNumeric(type))
            {
                if (filterOperator == FilterOperator.IsNull || filterOperator == FilterOperator.IsNotNull)
                {
                    return $"{property} {linqOperator} null";
                }
                else if (filterOperator == FilterOperator.IsEmpty || filterOperator == FilterOperator.IsNotEmpty)
                {
                    return $@"{property} {linqOperator} """"";
                }
                else
                {
                    return $"{property} {linqOperator} {filterValue}";
                }
            }
            else if (type == typeof(DateTime) ||
                    type == typeof(DateTime?) ||
                    type == typeof(DateTimeOffset) ||
                    type == typeof(DateTimeOffset?))
            {
                if (filterOperator == FilterOperator.IsNull || filterOperator == FilterOperator.IsNotNull)
                {
                    return $"{property} {linqOperator} null";
                }
                else if (filterOperator == FilterOperator.IsEmpty || filterOperator == FilterOperator.IsNotEmpty)
                {
                    return $@"{property} {linqOperator} """"";
                }
                else
                {
                    var dateTimeValue = DateTime.Parse(filterValue?.ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
                    var timezoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                    dateTimeValue = dateTimeValue - timezoneOffset;
                    var finalDate = dateTimeValue.TimeOfDay == TimeSpan.Zero ? dateTimeValue.Date : dateTimeValue;
                    var dateFunction = type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?) ? "DateTimeOffset" : "DateTime";
                    var date = TimeZoneInfo.ConvertTimeToUtc(finalDate);
                    return $@"{property} {linqOperator} {dateFunction}(""{date:yyyy-MM-ddTHH:mm:ssZ}"")";
                }
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                return $"{property} == {filterValue}";
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                if (filterOperator == FilterOperator.IsNull || filterOperator == FilterOperator.IsNotNull)
                {
                    return $"{property} {linqOperator} null";
                }
                else if (filterOperator == FilterOperator.IsEmpty || filterOperator == FilterOperator.IsNotEmpty)
                {
                    return $@"{property} {linqOperator} """"";
                }
                else
                {
                    return $@"{property} {linqOperator} Guid(""{filterValue}"")";
                }
            }

            return "";
        }

        private static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Wheres the specified filters.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dataFilter">The DataFilter.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable Where(this IQueryable source, IEnumerable<FilterDescriptor> filterDescriptors, Type type)
        {
            if (filterDescriptors.Where(canFilter).Any())
            {
                return source.Where(filterDescriptors.ToFilterString(type));
            }

            return source;
        }

        /// <summary>
        /// Selects the many recursive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            if (!result.Any())
            {
                return result;
            }
            return result.Concat(result.SelectManyRecursive(selector));
        }
    }
}