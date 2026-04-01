using Microsoft.EntityFrameworkCore;
using Q_A.DAL.Context;
using Q_A.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.DAL.Repository
{
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext DbContext = dbContext;
        protected readonly DbSet<T> DbSet = dbContext.Set<T>();

        public async Task<T?> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public IQueryable<T> Query()
        {
            return DbSet.AsQueryable();
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

       

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
