using System.Linq.Expressions;
using TestAuthenAndTextMessage.Ultilities;

namespace Accounting.Utilities
{
    public static class QueryableExtentions
    {
        /// <summary>
        /// Ordering by dynamic field.
        /// Parameter format: 'sortField_sortDirection'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="order">Ordering field and direction (format: 'sortField_sortDirection')</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> items, string order)
        {
            if (!string.IsNullOrEmpty(order) && order.Contains('_'))
            {
                var sortField = order.Split('_')[0];
                var sortDirection = order.Split('_')[1];
                var param = Expression.Parameter(typeof(T), "x");

                var prop = Expression.Property(param, sortField);

                var expression = Expression.Lambda(prop, param);

                string method = sortDirection.Equals("asc") ? "OrderBy" : "OrderByDescending";

                var types = new Type[] { items.ElementType, expression.Body.Type };

                var finalExpresstion = Expression.Call(typeof(Queryable), method, types, items.Expression, expression);
                return items.Provider.CreateQuery<T>(finalExpresstion);
            }
            return items;
        }

        /// <summary>
        /// Paginate list data for pagination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Pagination<T> Paginate<T>(this IQueryable<T> items, int pageIndex, int pageSize)
        {
            return new Pagination<T>(items.Count(), pageIndex, pageSize, items);
        }
    }
}
