using System.Collections;
using System.Collections.Generic;

namespace MyUtils.Pagination
{
    /// <summary>
    /// Non-generic contract representing a page of data.
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    public interface IPaginable : IEnumerable
    {
        /// <summary>
        /// Total number of pages.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        int TotalPageCount { get; }

        /// <summary>
        /// Total item count.
        /// </summary>
        /// <value>
        /// The total item count.
        /// </value>
        int TotalItemCount { get; }

        /// <summary>
        /// Current page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        int PageNumber { get; }

        /// <summary>
        /// Requested item count per page.
        /// </summary>
        /// <value>
        /// The item count per page.
        /// </value>
        int ItemCountPerPage { get; }

        /// <summary>
        /// Identifies the first page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first page; otherwise, <c>false</c>.
        /// </value>
        bool IsFirstPage { get; }

        /// <summary>
        /// Identifies the last page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        bool IsLastPage { get; }

        /// <summary>
        /// Identifies if there is a previous page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Identifies if there is a next page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        bool HasNextPage { get; }

        /// <summary>
        /// The first item number of the page.
        /// </summary>
        /// <value>
        /// The first item number.
        /// </value>
        int FirstItemNumber { get; }

        /// <summary>
        /// The last item number of this page.
        /// </summary>
        /// <value>
        /// The last item number.
        /// </value>
        int LastItemNumber { get; }
    }

    /// <summary>
    /// Generic contract representing a page of data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.IEnumerable" />
    public interface IPaginable<T> : IEnumerable<IPaginableItem<T>>, IPaginable
    {
        /// <summary>
        /// Gets the <see cref="IPaginableItem{T}"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IPaginableItem{T}"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IPaginableItem<T> this[int index] { get; }

        /// <summary>
        /// Number of items in this paginable.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }
    }
}