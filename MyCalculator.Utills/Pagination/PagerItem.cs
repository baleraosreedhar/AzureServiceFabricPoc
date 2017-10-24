namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPagerItem" />
    public class PagerItem : IPagerItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagerItem"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="totalPageCount">The total page count.</param>
        public PagerItem(int pageNumber, int totalPageCount)
        {
            PageNumber = pageNumber;
            TotalPageCount = totalPageCount;
        }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; }
        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        public int TotalPageCount { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is first page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first page; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstPage => PageNumber == 1;
        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastPage => PageNumber >= TotalPageCount;

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has previous page; otherwise, <c>false</c>.
        /// </value>
        public bool HasPreviousPage => PageNumber > 1;
        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has next page; otherwise, <c>false</c>.
        /// </value>
        public bool HasNextPage => PageNumber < TotalPageCount;
    }
}