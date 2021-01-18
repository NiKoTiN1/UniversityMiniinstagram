using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<ICollection<T>> Get(Expression<Func<T, bool>> filter, string[] children = null)
        {
            try
            {
                IQueryable<T> query = Set;
                if (children != null)
                {
                    foreach (var entity in children)
                    {
                        query = query.Include(entity);
                    }
                }
                return await query.Where(filter).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
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
    }
}
