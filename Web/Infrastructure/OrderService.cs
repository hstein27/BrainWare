using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    using System.Data;
    using Models;

    public class OrderService
    {
        public List<Order> GetOrdersForCompany(int CompanyId)
        {
            //HS - Using statement to ensure closure of underlying db connection
            using (var database = new Database())
            {
                // Get the orders
                //HS - get all order info in one query
                var sql = @"SELECT c.name, o.description, o.order_id, op.price,
                            op.product_id, op.quantity, p.name, p.price 
                            FROM company c INNER JOIN[order] o on c.company_id = o.company_id
                            inner join [orderproduct] op on o.order_id = op.order_id
                            INNER JOIN [product] p on op.product_id = p.product_id";

                //HS - move lists out of new Using statements so they are accessible later. Rename them for clarity.
                var orders = new Dictionary<int, Order>();
                var orderProducts = new List<OrderProduct>();
                //HS - Using statement will take care of  disposing
                using (var reader1 = database.ExecuteReader(sql))
                {
                    while (reader1.Read())
                    {
                        var rec = (IDataRecord)reader1;
                        int orderId = rec.GetInt32(2);
                        Order order = null;
                        if (!orders.ContainsKey(orderId))
                        {
                            order = new Order()
                            {
                                CompanyName = rec.GetString(0),
                                Description = rec.GetString(1),
                                OrderId = orderId,
                                OrderProducts = new List<OrderProduct>()
                            };
                            orders.Add(orderId, order);
                        }
                        else
                        {
                            order = orders[orderId];
                        }
                        //add product to OrderProducts list
                        var orderProduct = new OrderProduct()
                        {
                            OrderId = orderId,
                            Price = rec.GetDecimal(3),
                            ProductId = rec.GetInt32(4),
                            Quantity = rec.GetInt32(5),
                            Product = new Product()
                            {
                                Name = rec.GetString(6),
                                Price = rec.GetDecimal(7)
                            }
                        };
                        order.OrderProducts.Add(orderProduct);
                        order.OrderTotal += (orderProduct.Price * orderProduct.Quantity);
                    }
                    reader1.Close();
                }
                return orders.Values.ToList();
            }
        }
    }
}