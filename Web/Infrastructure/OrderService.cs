using System.Collections.Generic;
using System.Linq;

namespace Web.Infrastructure
{
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Models;

    /// <summary>
    /// Class for retrieving Orders 
    /// </summary>
    public class OrderService
    {
        const string CompanyIdParamName = "@companyId";
        const string OrderSql = "GetOrders";

        /// <summary>
        /// Retrieves Orders By Company ID
        /// </summary>
        /// <param name="CompanyId">ID of Company for Orders</param>
        /// <returns>List of orders for company</returns>
        public List<Order> GetOrdersForCompany(int CompanyId)
        {
            //HS - Using statement to ensure closure of underlying db connection
            using (var database = new Database())
            {
                // Get the orders
                //HS - get all order info in one query, using stored procedure. Need to add bind variable for company ID                               
                var orders = new Dictionary<int, Order>();
                var orderProducts = new List<OrderProduct>();
                SqlParameter companyIdParam = new SqlParameter(CompanyIdParamName, CompanyId)
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.Int32
                };
                SqlParameter[] paramArr = new SqlParameter[] { companyIdParam };
                //HS - Using statement will take care of  disposing
                using (var rdr = database.ExecuteReader(OrderSql, CommandType.StoredProcedure, paramArr))
                {
                    while (rdr.Read())
                    {
                        CreateOrder(orders, rdr);
                    }
                    rdr.Close();
                }
                return orders.Values.ToList();
            }
        }

        /// <summary>
        /// Create order and add to list
        /// </summary>
        /// <param name="orders">List of orders to add to</param>
        /// <param name="rdr">DataReader to read orders from</param>
        private void CreateOrder(Dictionary<int, Order> orders, DbDataReader rdr)
        {
            int orderId = rdr.GetInt32(2);
            Order order;
            if (!orders.ContainsKey(orderId))
            {
                order = new Order()
                {
                    CompanyName = rdr.GetString(0),
                    Description = rdr.GetString(1),
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
                Price = rdr.GetDecimal(3),
                ProductId = rdr.GetInt32(4),
                Quantity = rdr.GetInt32(5),
                Product = new Product()
                {
                    Name = rdr.GetString(6),
                    Price = rdr.GetDecimal(7)
                }
            };
            order.OrderProducts.Add(orderProduct);
            order.OrderTotal += (orderProduct.Price * orderProduct.Quantity);
        }
    }
}