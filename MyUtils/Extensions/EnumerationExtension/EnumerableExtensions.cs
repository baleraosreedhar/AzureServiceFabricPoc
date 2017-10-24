using MyUtils.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyUtils.Extensions.EnumerationExtension
{
    /// <summary>
    /// Enumaerable Extensions
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// To the paginable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemCountPerPage">The item count per page.</param>
        /// <returns></returns>
        public static IPaginable<T> ToPaginable<T>(this IEnumerable<T> enumerable, int pageNumber, int itemCountPerPage)
        {
            return
                enumerable
                    .AsQueryable()
                    .ToPaginable(pageNumber, itemCountPerPage);
        }

        /// <summary>
        /// To the paginable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="paginableRequest">The paginable request.</param>
        /// <returns></returns>
        public static IPaginable<T> ToPaginable<T>(this IEnumerable<T> enumerable, IPaginableRequest paginableRequest)
        {
            return
                enumerable
                    .ToPaginable(paginableRequest.PageNumber, paginableRequest.ItemCountPerPage);
        }

        /// <summary>
        /// To the paginable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemCountPerPage">The item count per page.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <returns></returns>
        public static IPaginable<T> ToPaginable<T>(this IEnumerable<T> enumerable, int pageNumber, int itemCountPerPage,
            int totalItemCount)
        {
            //# If enumerable is EF Queryable instance, a '.ToList()' will force it to execute the SQL.
            return new StaticPaginable<T>(enumerable.ToList(), pageNumber, itemCountPerPage, totalItemCount);
        }

        /// <summary>
        /// Distincts the by.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> hashSet = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (hashSet.Add(keySelector(element))) { yield return element; }
            }
        }
    }
}