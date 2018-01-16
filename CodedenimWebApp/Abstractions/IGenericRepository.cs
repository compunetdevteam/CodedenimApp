using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodedenimWebApp.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<object> GetWhere(Expression<Func<T, bool>> predicate);
    }
}
