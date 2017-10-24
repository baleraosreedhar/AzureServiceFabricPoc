using System.Collections.Generic;

namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPager" />
    public class StaticPager : IPager
    {
        /// <summary>
        /// The pages
        /// </summary>
        private readonly IList<IPagerItem> pages;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticPager"/> class.
        /// </summary>
        /// <param name="paginable">The paginable.</param>
        /// <param name="maximumPageNumberCount">The maximum page number count.</param>
        public StaticPager(IPaginable paginable, int maximumPageNumberCount)
        {
            MaximumPageNumberCount = maximumPageNumberCount;
            TotalPageCount = paginable.TotalPageCount;

            var firstPageToDisplay = 1;
            var lastPageToDisplay = paginable.TotalPageCount;
            var pageNumbersToDisplay = lastPageToDisplay;

            if (paginable.TotalPageCount > maximumPageNumberCount)
            {
                var maxPageNumbersToDisplay = maximumPageNumberCount;
                firstPageToDisplay = paginable.PageNumber - maxPageNumbersToDisplay/2;
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > paginable.TotalPageCount)
                    firstPageToDisplay = paginable.TotalPageCount - maxPageNumbersToDisplay + 1;
            }

            var totalPageNumber = paginable.TotalPageCount;
            if (totalPageNumber == 0)
                totalPageNumber = 1;

            pages = new List<IPagerItem>();

            for (var i = firstPageToDisplay; i <= firstPageToDisplay + pageNumbersToDisplay - 1; i++)
                pages.Add(new PagerItem(i, totalPageNumber));
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IPagerItem> IPager.GetPages()
        {
            return pages;
        }

        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        public int TotalPageCount { get; }
        /// <summary>
        /// Gets the maximum page number count.
        /// </summary>
        /// <value>
        /// The maximum page number count.
        /// </value>
        public int MaximumPageNumberCount { get; }
    }
}