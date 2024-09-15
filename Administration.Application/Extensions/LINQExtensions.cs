using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.Extensions
{
    /// <summary>
    /// Extension class used to hold extension methods
    /// </summary>
    public static class LINQExtensions
    {
        /// <summary>
        /// If conditionals inside queries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="should"></param>
        /// <param name="transforms"></param>
        /// <returns></returns>
        public static IQueryable<T> If<T>(
            this IQueryable<T> query,
            bool should,
            params Func<IQueryable<T>, IQueryable<T>>[] transforms)
        {
            return should
                ? transforms.Aggregate(query,
                    (current, transform) => transform.Invoke(current))
                : query;
        }

        public static IEnumerable<T> If<T>(
            this IEnumerable<T> query,
            bool should,
            params Func<IEnumerable<T>, IEnumerable<T>>[] transforms)
        {
            return should
                ? transforms.Aggregate(query,
                    (current, transform) => transform.Invoke(current))
                : query;
        }
    }
}
