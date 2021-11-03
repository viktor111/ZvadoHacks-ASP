using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZvadoHacks.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> 
        where T : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected GenericRepository
            (
                ApplicationDbContext dbContext
            )
        {
            DbContext = dbContext;
        }

        public virtual async Task<T> Add(T entity)
        {
            var result = await DbContext.AddAsync(entity);
            await SaveChanges();
            return result.Entity;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            var result = await DbContext.Set<T>()
                .ToListAsync();

            return result;
        }

        public virtual async Task<T> Delete(T entity)
        {
            var deleteTask = Task.Run(() =>
            {
                DbContext.Remove(entity);
                return entity;
            });

            await DbContext.SaveChangesAsync();

            return await deleteTask;
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            var result = await DbContext.Set<T>()
                .AsQueryable()
                .Where(predicate)
                .ToListAsync();

            return result;
        }

        public virtual async Task<T> Get(Guid id)
        {
            var result = await DbContext.FindAsync<T>(id);

            return result;
        }

        public virtual async Task<T> GetByProperty(Expression<Func<T, bool>> predicate)
        {
            var genericDb = DbContext.Set<T>();

            var result = await genericDb.FirstOrDefaultAsync(predicate);

            return result;
        }

        protected virtual async Task<bool> SaveChanges()
        {
            return (await DbContext.SaveChangesAsync()) > 0;
        }

        public virtual async Task<T> Update(T entity)
        {
            var updateTask = Task.Run(() =>
            {
                var updatedEntity = DbContext.Update(entity).Entity;

                return updatedEntity;
            });

            await SaveChanges();

            return await updateTask;
        }
    }
}
