using System.Collections.Generic;

namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPager
    {
        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        int TotalPageCount { get; }
        /// <summary>
        /// Gets the maximum page number count.
        /// </summary>
        /// <value>
        /// The maximum page number count.
        /// </value>
        int MaximumPageNumberCount { get; }
        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPagerItem> GetPages();
    }
}