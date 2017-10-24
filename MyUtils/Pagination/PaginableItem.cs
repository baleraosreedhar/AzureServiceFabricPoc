namespace MyUtils.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MyPortfolio.Domain.Pagination.IPaginableItem{T}" />
    public class PaginableItem<T> : IPaginableItem<T>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="PaginableItem{T}"/> class from being created.
        /// </summary>
        private PaginableItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginableItem{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemNumber">The item number.</param>
        public PaginableItem(T item, int itemNumber)
        {
            Item = item;
            ItemNumber = itemNumber;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; }
        /// <summary>
        /// Gets the item number.
        /// </summary>
        /// <value>
        /// The item number.
        /// </value>
        public int ItemNumber { get; }
    }
}