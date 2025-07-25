using BasicForm.Common.DB.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.DAL.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public async Task<TEntity> GetByLongId(long id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<bool> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddAllAsync(List<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);

            return await _dbContext.SaveChangesAsync() >= entities.Count;
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAllAsync(List<TEntity> entities)
        {
            _dbContext.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() >= entities.Count();
        }
    }
}
