using System.Collections.Generic;
using System.Web.Http;

namespace Web.Controllers
{
    using System.Web.Mvc;
    using Infrastructure;
    using Models;

    /// <summary>
    /// Class for retrieving Orders via API
    /// </summary>
    public class OrderController : ApiController
    {
        /// <summary>
        /// Retrieve orders by company ID
        /// </summary>
        /// <param name="id">Company ID for order retrieval</param>
        /// <returns>List of orders for company ID</returns>
        [HttpGet]
        public IEnumerable<Order> GetOrders(int id = 1)
        {
            var data = new OrderService();

            return data.GetOrdersForCompany(id);
        }
    }
}
