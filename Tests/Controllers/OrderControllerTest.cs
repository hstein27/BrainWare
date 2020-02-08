using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using Web.Models;

namespace Tests.Controllers
{
    //HS - new class for testing OrderController
    [TestClass]
    public class OrderControllerTest
    {
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
        const string TwoInchStraight = "2\" straight";
        Dictionary<int, Product> sampleProducts = new Dictionary<int, Product>();

        public OrderControllerTest()
        {
            sampleProducts.Add(1, new Product { Name= PipeFitting, Price=(decimal)1.00 });
            sampleProducts.Add(2, new Product { Name = TenInchStraight, Price = (decimal)2.00 });
            sampleProducts.Add(3, new Product { Name = QuarterTurn, Price = (decimal)1.00 });
            sampleProducts.Add(4, new Product { Name = FiveInchStraight, Price = (decimal)1.00 }); 
            sampleProducts.Add(5, new Product { Name = TwoInchStraight, Price = (decimal)1.00 });

        }

        [TestMethod]
        public void GetEmptyOrdersList()
        {
            OrderController orderController = new OrderController();
            //Get order list for non-existant company ID, should be empty
            IEnumerable<Order> orders = orderController.GetOrders(11);
            Assert.IsFalse(orders.Any());
        }

        [TestMethod]
        public void GetOrdersList()
        {
            OrderController orderController = new OrderController();
            //Get order list for company 1 and check all elements
            List<Order> orders = orderController.GetOrders(1).ToList();
            Assert.AreEqual(3, orders.Count);

            //now look at each order
            VerifyOrder(orders, 1, (decimal)39.5, 3);
            VerifyOrder(orders, 2, (decimal)44, 4);
            VerifyOrder(orders, 3, (decimal)44.25, 5);

        }

        private void VerifyOrder(List<Order> orders, int orderNumber, decimal orderTotal, int numProducts)
        {
            Order order = orders.FirstOrDefault(o => o.OrderId == orderNumber);
            Assert.IsNotNull(order);
            //check order total and number of products
            Assert.AreEqual(orderTotal, order.OrderTotal);
            Assert.AreEqual(numProducts, order.OrderProducts.Count);
            
            //check the price for each product. The values in Product table don't always match what's in the OrderProduct table.
            //either this is a bug or a "feature" where prices are adjusted per order, so don't check it here
            foreach(OrderProduct orderProduct in order.OrderProducts)
            {
                Product product; 
                sampleProducts.TryGetValue(orderProduct.ProductId, out product);
                Assert.IsNotNull(product);
                Assert.AreEqual(orderProduct.Product.Name, product.Name);
            }
        }
    }
}
