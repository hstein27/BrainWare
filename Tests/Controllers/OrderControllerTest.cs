using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using Web.Models;

namespace Tests.Controllers
{
    //HS - new class for testing OrderController
    /// <summary>
    /// Class for testing OrderController
    /// </summary>
    [TestClass]
    public class OrderControllerTest
    {
        /// <summary>
        /// Structure for mapping product to price and quantity on an order
        /// </summary>
        struct ProductPriceQuantity
        {
            public int Quantity;
            public decimal Price;
        }
        //Product list in sample db
        //1	Pipe fitting	1.00
        //2	10" straight	2.00
        //3	Quarter turn	1.00
        //4	5" straight	1.00
        //5	2" stright	1.00
        const string PipeFitting = "Pipe fitting";
        const string TenInchStraight = "10\" straight";
        const string QuarterTurn = "Quarter turn";
        const string FiveInchStraight = "5\" straight";
        const string TwoInchStraight = "2\" straight";//this was misspelled in sample DB, now it is corrected
        readonly Dictionary<int, Product> sampleProducts = new Dictionary<int, Product>();

        /// <summary>
        /// Initializes sample product list for test
        /// </summary>
        public OrderControllerTest()
        {
            sampleProducts.Add(1, new Product { Name= PipeFitting, Price=(decimal)1.00 });
            sampleProducts.Add(2, new Product { Name = TenInchStraight, Price = (decimal)2.00 });
            sampleProducts.Add(3, new Product { Name = QuarterTurn, Price = (decimal)1.00 });
            sampleProducts.Add(4, new Product { Name = FiveInchStraight, Price = (decimal)1.00 }); 
            sampleProducts.Add(5, new Product { Name = TwoInchStraight, Price = (decimal)1.00 });

        }

        /// <summary>
        /// Retrieve empty order list
        /// </summary>
        [TestMethod]
        public void GetEmptyOrdersList()
        {
            OrderController orderController = new OrderController();
            //Get order list for non-existant company ID, should be empty
            IEnumerable<Order> orders = orderController.GetOrders(11);
            Assert.IsFalse(orders.Any());
        }

        /// <summary>
        /// Get all orders for company ID = 1
        /// </summary>
        [TestMethod]
        public void GetOrdersList()
        {
            OrderController orderController = new OrderController();
            //Get order list for company 1 and check all elements
            List<Order> orders = orderController.GetOrders(1).ToList();
            Assert.AreEqual(3, orders.Count);

            //set up dictionary for each order, to map quantity and prices
            Dictionary<int, ProductPriceQuantity> orderOneProducts = new Dictionary<int, ProductPriceQuantity>
            {
                { 1, new ProductPriceQuantity { Quantity = 10, Price = (decimal)1.23 } },
                { 3, new ProductPriceQuantity { Quantity = 3, Price = 1 } },
                { 4, new ProductPriceQuantity { Quantity = 22, Price = (decimal)1.1 } }
            };

            Dictionary<int, ProductPriceQuantity> orderTwoProducts = new Dictionary<int, ProductPriceQuantity>
            {
                { 1, new ProductPriceQuantity { Quantity = 10, Price = (decimal)1.23 } },
                { 2, new ProductPriceQuantity { Quantity = 13, Price = 2 } },
                { 3, new ProductPriceQuantity { Quantity = 3, Price = 1 } },
                { 5, new ProductPriceQuantity { Quantity = 3, Price = (decimal)0.9 } }
            };

            Dictionary<int, ProductPriceQuantity> orderThreeProducts = new Dictionary<int, ProductPriceQuantity>
            {
                { 1, new ProductPriceQuantity { Quantity = 10, Price = (decimal)1.23 } },
                { 2, new ProductPriceQuantity { Quantity = 7, Price = 2 } },
                { 3, new ProductPriceQuantity { Quantity = 13, Price = (decimal)0.75 } },
                { 4, new ProductPriceQuantity { Quantity = 5, Price = (decimal)1.1 } },
                { 5, new ProductPriceQuantity { Quantity = 3, Price = (decimal)0.9 } }
            };
            
            //now verify at each order
            VerifyOrder(orders, 1, (decimal)39.5, orderOneProducts);
            VerifyOrder(orders, 2, 44, orderTwoProducts);
            VerifyOrder(orders, 3, (decimal)44.25, orderThreeProducts);

        }

        /// <summary>
        /// Verify order matches expected values
        /// </summary>
        /// <param name="orders">List of all orders</param>
        /// <param name="orderNumber">ID of order to verify</param>
        /// <param name="orderTotal">Expected order price total</param>
        /// <param name="productQuantities">Mapping of product quantities and totals for order</param>
        private void VerifyOrder(List<Order> orders, int orderNumber, decimal orderTotal,  Dictionary<int, ProductPriceQuantity> productQuantities)
        {
            Order order = orders.FirstOrDefault(o => o.OrderId == orderNumber);
            Assert.IsNotNull(order);
            //check order total and number of products
            Assert.AreEqual(orderTotal, order.OrderTotal);
            Assert.AreEqual(productQuantities.Count, order.OrderProducts.Count);
            
            //check the price for each product. The values in Product table don't always match what's in the OrderProduct table.
            //this could be because of price fluctuations over time, so don't check product price in order vs. price in Product table
            foreach(OrderProduct orderProduct in order.OrderProducts)
            {
                sampleProducts.TryGetValue(orderProduct.ProductId, out Product product);
                Assert.IsNotNull(product);
                Assert.AreEqual(orderProduct.Product.Name, product.Name);
                //now verify product quantities
                Assert.IsTrue(productQuantities.ContainsKey(orderProduct.ProductId));
                Assert.AreEqual(productQuantities[orderProduct.ProductId].Quantity, orderProduct.Quantity);
                Assert.AreEqual(productQuantities[orderProduct.ProductId].Price, orderProduct.Price);
            }
        }
    }
}
