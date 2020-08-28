using Marquesita.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using Marquesita.Infrastructure.Interfaces;

namespace Marquesita.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected BusinessDbContext _context;

        public GenericRepository(BusinessDbContext context)
        {
            _context = context;
        }

        public virtual T Add(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>().ToList();
        }

        public virtual IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual T Get(Guid id)
        {
            return _context.Find<T>(id);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            return _context.Update(entity).Entity;
        }

        public virtual void Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}
