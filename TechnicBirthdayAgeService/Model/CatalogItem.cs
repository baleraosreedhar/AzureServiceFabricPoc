using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicBirthdayAgeService.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CatalogDomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDomainException"/> class.
        /// </summary>
        public CatalogDomainException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDomainException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CatalogDomainException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDomainException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CatalogDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CatalogBrand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>
        /// The brand.
        /// </value>
        public string Brand { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CatalogType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CatalogItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the name of the picture file.
        /// </summary>
        /// <value>
        /// The name of the picture file.
        /// </value>
        public string PictureFileName { get; set; }

        /// <summary>
        /// Gets or sets the picture URI.
        /// </summary>
        /// <value>
        /// The picture URI.
        /// </value>
        public string PictureUri { get; set; }

        /// <summary>
        /// Gets or sets the catalog type identifier.
        /// </summary>
        /// <value>
        /// The catalog type identifier.
        /// </value>
        public int CatalogTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the catalog.
        /// </summary>
        /// <value>
        /// The type of the catalog.
        /// </value>
        public CatalogType CatalogType { get; set; }

        /// <summary>
        /// Gets or sets the catalog brand identifier.
        /// </summary>
        /// <value>
        /// The catalog brand identifier.
        /// </value>
        public int CatalogBrandId { get; set; }

        /// <summary>
        /// Gets or sets the catalog brand.
        /// </summary>
        /// <value>
        /// The catalog brand.
        /// </value>
        public CatalogBrand CatalogBrand { get; set; }

        // Quantity in stock
        /// <summary>
        /// Gets or sets the available stock.
        /// </summary>
        /// <value>
        /// The available stock.
        /// </value>
        public int AvailableStock { get; set; }

        // Available stock at which we should reorder
        /// <summary>
        /// Gets or sets the restock threshold.
        /// </summary>
        /// <value>
        /// The restock threshold.
        /// </value>
        public int RestockThreshold { get; set; }


        // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
        /// <summary>
        /// Gets or sets the maximum stock threshold.
        /// </summary>
        /// <value>
        /// The maximum stock threshold.
        /// </value>
        public int MaxStockThreshold { get; set; }

        /// <summary>
        /// True if item is on reorder
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on reorder]; otherwise, <c>false</c>.
        /// </value>
        public bool OnReorder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItem"/> class.
        /// </summary>
        public CatalogItem() { }


        /// <summary>
        /// Decrements the quantity of a particular item in inventory and ensures the restockThreshold hasn't
        /// been breached. If so, a RestockRequest is generated in CheckThreshold.
        /// If there is sufficient stock of an item, then the integer returned at the end of this call should be the same as quantityDesired.
        /// In the event that there is not sufficient stock available, the method will remove whatever stock is available and return that quantity to the client.
        /// In this case, it is the responsibility of the client to determine if the amount that is returned is the same as quantityDesired.
        /// It is invalid to pass in a negative number.
        /// </summary>
        /// <param name="quantityDesired">The quantity desired.</param>
        /// <returns>
        /// int: Returns the number actually removed from stock.
        /// </returns>
        /// <exception cref="TechnicBirthdayAgeService.Model.CatalogDomainException">
        /// </exception>
        public int RemoveStock(int quantityDesired)
        {
            if (AvailableStock == 0)
            {
                throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
            }

            if (quantityDesired <= 0)
            {
                throw new CatalogDomainException($"Item units desired should be greater than cero");
            }

            int removed = Math.Min(quantityDesired, this.AvailableStock);

            this.AvailableStock -= removed;

            return removed;
        }

        /// <summary>
        /// Increments the quantity of a particular item in inventory.
        /// <param name="quantity"></param><returns>int: Returns the quantity that has been added to stock</returns>
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <returns></returns>
        public int AddStock(int quantity)
        {
            int original = this.AvailableStock;

            // The quantity that the client is trying to add to stock is greater than what can be physically accommodated in the Warehouse
            if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
            {
                // For now, this method only adds new units up maximum stock threshold. In an expanded version of this application, we
                //could include tracking for the remaining units and store information about overstock elsewhere. 
                this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
            }
            else
            {
                this.AvailableStock += quantity;
            }

            this.OnReorder = false;

            return this.AvailableStock - original;
        }
    }
}
