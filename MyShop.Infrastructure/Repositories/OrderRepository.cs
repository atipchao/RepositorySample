using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyShop.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ShoppingContext context) : base(context)
        {
        }

        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            //return base.Find(predicate);
            return context.Orders
                .Include(o => o.LineItems)
                .ThenInclude(l => l.Product)
                .Where(predicate)
                .ToList();
        }

        public override Order Update(Order entity)
        {
            //locate the target order with its line-items
            var order = context.Orders
                .Include(o => o.LineItems)
                .ThenInclude(l => l.Product)
                .Single(s => s.OrderId == entity.OrderId);

            order.OrderDate = entity.OrderDate;
            order.LineItems = entity.LineItems;

            return base.Update(entity);
        }
    }
}
