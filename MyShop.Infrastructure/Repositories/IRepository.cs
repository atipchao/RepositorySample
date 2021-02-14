using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyShop.Infrastructure.Repositories
{
    public interface IRepository<T> //Generic interface. This is used in our Generic-Repository
    {
        // This generic interface allows Create, Update, Read, Get-all-items, 
        // get-items-with-criteria
        T Add(T entity);
        T Update(T entity);
        T Get(Guid id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate); // Note, predicate is a LINQ select criteria
        void SaveChanges();
    }
}
