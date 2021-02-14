using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Controllers;
using MyShop.Web.Models;
using System;

namespace MyShop.Web.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var orderRepository = new Mock<IRepository<Order>>();
            var productRepository = new Mock<IRepository<Product>>();

            var orderComtroller = new OrderController(
                orderRepository.Object,
                productRepository.Object
                );
            //
            // Now cretae a TEST data - for testing
            //

            var createOrderModel = new CreateOrderModel
            {
                //Customer
                Customer = new CustomerModel
                {
                    Name = "Test Lastname",
                    ShippingAddress = "123 Test Address AVE",
                    City = "Chicago",
                    Country = "USA",
                    PostalCode = "60602"

                },
                LineItems = new[]
                {
                     new LineItemModel{ ProductId = Guid.NewGuid(), Quantity = 2 },
                     new LineItemModel{ ProductId = Guid.NewGuid(), Quantity = 10 }
                }                
            };

            orderComtroller.Create(createOrderModel);

            // Call TEST
            orderRepository.Verify(repository => repository.Add(It.IsAny<Order>()),
                Times.AtLeastOnce());
        }
    }
}
