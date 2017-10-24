namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPaginableRequest" />
    public class PaginableRequest : IPaginableRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginableRequest"/> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemCountPerPage">The item count per page.</param>
        public PaginableRequest(int pageNumber, int itemCountPerPage)
        {
            PageNumber = pageNumber;
            ItemCountPerPage = itemCountPerPage;
        }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; }
        /// <summary>
        /// Gets the item count per page.
        /// </summary>
        /// <value>
        /// The item count per page.
        /// </value>
        public int ItemCountPerPage { get; }
    }
}