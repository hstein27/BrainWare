using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;

namespace Tests.Controllers
{
    /// <summary>
    /// Test for testing HomeController
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        /// <summary>
        /// Verify Index method returns correct tile
        /// </summary>
        [TestMethod]
        public void Index()
        {
            // Arrange
            using (HomeController controller = new HomeController())
            {

                // Act
                ViewResult result = controller.Index() as ViewResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Home Page", result.ViewBag.Title);
            }
        }
    }
}
