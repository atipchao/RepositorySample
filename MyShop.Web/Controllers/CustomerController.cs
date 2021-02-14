using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;

namespace MyShop.Web.Controllers
{
    public class CustomerController : Controller
    {
        //Old
        //private ShoppingContext context;
        //New
        private readonly IRepository<Customer> _customerrepository;

        public CustomerController(IRepository<Customer> customerRepository)
        {
            //Old
            //context = new ShoppingContext();
            this._customerrepository = customerRepository;
        }

        public IActionResult Index(Guid? id)
        {
            if (id == null)
            {
                //Old
                //var customers = context.Customers.ToList();
                var customers = _customerrepository.All();

                return View(customers);
            }
            else
            {
                //Old
                //var customer = context.Customers.Find(id.Value);
                var customer = _customerrepository.Get(id.Value);

                return View(new[] { customer }); // this is where I don't like - why we need new[]
            }
        }
    }
}
