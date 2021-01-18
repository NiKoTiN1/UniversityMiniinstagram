using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task Add(T item);
        public Task<ICollection<T>> Get(Expression<Func<T, bool>> filter, string[] children = null);
        public Task Update(T item);
        public Task Remove(T item);
    }
}
