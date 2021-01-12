using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
    {
        public BaseRepository(DatabaseContext context)
        {
            this.Context = context;
        }

        private DbSet<T> Set => this.Context.Set<T>();

        private readonly DatabaseContext Context;
        public async Task Add(T item)
        {
            Set.Add(item);
            await this.Context.SaveChangesAsync();
        }

        public async Task<T> Get(string Id)
        {
            T FoundElem = await Set.FirstOrDefaultAsync(item => item.Id == Id);
            return FoundElem;
        }

        public async Task Remove(T item)
        {
            this.Context.Remove(item);
            await this.Context.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            this.Context.Update(item);
            await this.Context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await Set.ToListAsync();
        }
    }
}
