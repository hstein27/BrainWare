using System.Collections.Generic;

namespace Web.Models
{
    /// <summary>
    /// Internal class for Constant values
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// Format to two decimal places
        /// </summary>
        internal const string DecimalFormat = "F2";
    }

    /// <summary>
    /// Class representing an Order in the database
    /// </summary>
    public class Order
    {
        /// <summary>
        /// ID of Order
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Company name for Order
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Description of order
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Dollar amount for order total
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Formatted amount for this order - print this with two decimal places
        /// </summary>
        public string FormattedOrderTotal
        {
            get
            {
                return OrderTotal.ToString(Constants.DecimalFormat);
            }
        }

        /// <summary>
        /// List of products for this Order
        /// </summary>
        public List<OrderProduct> OrderProducts { get; set; }

    }

    /// <summary>
    /// Class representing a product on an Order
    /// </summary>
    public class OrderProduct
    {
        /// <summary>
        /// ID of the Order for this product
        /// </summary>
        public int OrderId { get; set; }
        
        /// <summary>
        /// Product ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Instance of Product with more info
        /// </summary>
        public Product Product { get; set; }
    
        /// <summary>
        /// Number of units of this Product
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price for this product per unit, in dollars
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Formatted price for this product - print this with two decimal places
        /// </summary>
        public string FormattedPrice
        {
            get
            {
                return Price.ToString(Constants.DecimalFormat);
            }
        }
    }

    /// <summary>
    /// Class representing a Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Produc price in dollars
        /// </summary>
        public decimal Price { get; set; }
    }
}