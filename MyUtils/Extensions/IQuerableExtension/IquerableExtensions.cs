using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyUtils.Extensions.IQuerableExtension
{
    /// <summary>
    /// IQueryableExtension
    /// </summary>
    ////[CLSCompliant(true)]
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Finds first item in IQueryable, else returns new() constructed item
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="item">Item to search</param>
        /// <returns>First item in IQueryable, else returns new() constructed item</returns>
        public static T FirstOrDefaultSafe<T>(this IQueryable<T> item) where T : new()
        {
            return (item != null && item.FirstOrDefault() != null) ? item.FirstOrDefault() : new T();
        }

        /// <summary>
        /// Finds last item in IQueryable, else returns new() constructed item
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="item">Item to search</param>
        /// <returns>First item in IQueryable, else returns new() constructed item</returns>
        public static T LastOrDefaultSafe<T>(this IQueryable<T> item) where T : new()
        {
            return (item != null && item.LastOrDefault() != null) ? item.LastOrDefault() : new T();
        }
    }
}
