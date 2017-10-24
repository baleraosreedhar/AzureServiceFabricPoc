namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPaginableRequest
    {
        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        int PageNumber { get; }
        /// <summary>
        /// Gets the item count per page.
        /// </summary>
        /// <value>
        /// The item count per page.
        /// </value>
        int ItemCountPerPage { get; }
    }
}