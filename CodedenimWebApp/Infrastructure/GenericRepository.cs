using CodedenimWebApp.Abstractions;
using CodedenimWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CodedenimWebApp.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        private DbSet<T> set;

        public GenericRepository(ApplicationDbContext db)
        {
            _context = db;
            set = _context.Set<T>();
        }


        public IEnumerable<object> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return set.Where(predicate);
        }
    }
}