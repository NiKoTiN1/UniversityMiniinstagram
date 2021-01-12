using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IBaseReposetry<T>
    {
        public Task Add(T item);
        public Task<T> Get(string Id);
        public Task Update(T item);
        public Task Remove(T item);
        public Task<List<T>> GetAll();
    }
}
