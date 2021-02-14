using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class OrderController : Controller
    {
        //private ShoppingContext context;  // old before change
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Customer> _customerrepository;



        public OrderController(IRepository<Order> orderRepository
            , IRepository<Product> productRepository
            , IRepository<Customer> customerrepository)
        {
            // context = new ShoppingContext(); //Before 
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._customerrepository = customerrepository;
        }

        public IActionResult Index()
        {
            //Old Before
            //var orders = context.Orders
            //    .Include(order => order.LineItems)
            //    .ThenInclude(lineItem => lineItem.Product)
            //    .Where(order => order.OrderDate > DateTime.UtcNow.AddDays(-1)).ToList();

            var orders = _orderRepository.Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1)).ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            //Old before
            //var products = context.Products.ToList();
            var products = _productRepository.All();
            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");

            //Get the customer

            var customer = _customerrepository
                .Find(c => c.Name == model.Customer.Name)
                .FirstOrDefault();

            //If customer found
            if (customer != null)
            {
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.PostalCode = model.Customer.PostalCode;
                customer.City = model.Customer.City;
                customer.Country = model.Customer.Country;

                _customerrepository.Update(customer);
                _customerrepository.SaveChanges();

            }
            else
            {

                customer = new Customer
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            //Old
            //context.Orders.Add(order);
            _orderRepository.Add(order);

            //Old
            //context.SaveChanges();
            _orderRepository.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
