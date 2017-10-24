using System;
using System.Collections;
using System.Collections.Generic;
using MyUtils.Pagination;

namespace MyUtils.Pagination
{
    /// <summary>
    /// Base class represents page of items.
    /// </summary>
    /// <typeparam name="T">The type of items in this paginable.</typeparam>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPaginable{T}" />
    public abstract class Paginable<T> : IPaginable<T>
    {
        /// <summary>
        /// The inner list
        /// </summary>
        protected readonly List<IPaginableItem<T>> innerList = new List<IPaginableItem<T>>();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IPaginableItem<T>> IEnumerable<IPaginableItem<T>>.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        /// <summary>
        /// Total number of items.
        /// </summary>
        /// <value>
        /// The total item count.
        /// </value>
        public int TotalItemCount { get; protected set; }

        /// <summary>
        /// Requested page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; protected set; }

        /// <summary>
        /// Requested number of items per page.
        /// </summary>
        /// <value>
        /// The item count per page.
        /// </value>
        public int ItemCountPerPage { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IPaginableItem{T}"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IPaginableItem{T}"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IPaginableItem<T> IPaginable<T>.this[int index] => innerList[index];

        /// <summary>
        /// Number of items in this result set.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count => innerList.Count;

        /// <summary>
        /// Total number of pages.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        public int TotalPageCount =>
            TotalItemCount > 0 ? (int) Math.Ceiling(TotalItemCount/(double) ItemCountPerPage) : 0;

        /// <summary>
        /// Identifies the first page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first page; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstPage => PageNumber == 1;

        /// <summary>
        /// Identifies the last page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastPage => PageNumber >= TotalPageCount;

        /// <summary>
        /// Identifies if a previous page is available.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Identifies if a next page is available.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage => PageNumber < TotalPageCount;

        /// <summary>
        /// Identifies the first item number of the page.
        /// </summary>
        /// <value>
        /// The first item number.
        /// </value>
        public int FirstItemNumber { get; protected set; }

        /// <summary>
        /// Identifies the last item number of the page.
        /// </summary>
        /// <value>
        /// The last item number.
        /// </value>
        public int LastItemNumber { get; protected set; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPaginable{T}" />
    public static class Paginable
    {
        /// <summary>
        /// Returns new empty paginable collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IPaginable<T> Empty<T>()
        {
            return new EmptyPaginable<T>();
        }
    }
}