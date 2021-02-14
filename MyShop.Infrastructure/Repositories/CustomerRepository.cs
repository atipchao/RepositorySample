using MyShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShop.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(ShoppingContext context) : base(context)
        {
        }

        public override Customer Update(Customer entity)
        {
            var cust = context.Customers
                .Single(s => s.CustomerId == entity.CustomerId);
            cust.Name = entity.Name;
            cust.City = entity.City;
            cust.PostalCode = entity.PostalCode;
            cust.ShippingAddress = entity.ShippingAddress;
            cust.Country = entity.Country;
            return base.Update(cust);
        }
    }
}
