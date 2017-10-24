using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUtils.Pagination
{
    /// <summary>
    /// Encapsulates a collection of data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MyPortfolio.Domain.Pagination.Paginable{T}" />
    public class StaticPaginable<T> : Paginable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPaginable{T}"/> class.
        /// </summary>
        /// <param name="subset">The subset.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemCountPerPage">The item count per page.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// pageNumber
        /// or
        /// itemCountPerPage
        /// or
        /// totalItemCount
        /// </exception>
        public StaticPaginable(IEnumerable<T> subset, int pageNumber, int itemCountPerPage, int totalItemCount)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber));

            if (itemCountPerPage < 1)
                throw new ArgumentOutOfRangeException(nameof(itemCountPerPage));

            if (subset.Count() > totalItemCount)
                throw new ArgumentOutOfRangeException(nameof(totalItemCount));

            TotalItemCount = totalItemCount;
            PageNumber = pageNumber;
            ItemCountPerPage = itemCountPerPage;

            innerList.AddRange(subset.ToPaginableItemList(pageNumber, itemCountPerPage));

            if (innerList.Any())
            {
                FirstItemNumber = innerList.First().ItemNumber;
                LastItemNumber = innerList.Last().ItemNumber;
            }
            else
            {
                FirstItemNumber = 0;
                LastItemNumber = 0;
            }
        }
    }
}