namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPagerItem
    {
        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        int PageNumber { get; }
        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        int TotalPageCount { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is first page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first page; otherwise, <c>false</c>.
        /// </value>
        bool IsFirstPage { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        bool IsLastPage { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        bool HasPreviousPage { get; }
        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        bool HasNextPage { get; }
    }
}