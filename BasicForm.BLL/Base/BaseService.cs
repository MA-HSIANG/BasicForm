using BasicForm.DAL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.BLL.Base
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private readonly IBaseRepository<TEntity> _repository;
        public BaseService(IBaseRepository<TEntity> repository) 
        { 
            _repository = repository;
        }
        public async Task<TEntity> GetByLongId(long id)
        {
            return await _repository.GetByLongId(id);
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<bool> AddAsync(TEntity entity)
        {
            return await _repository.AddAsync(entity);  
        }
        public async Task<bool> AddAllAsync(List<TEntity> entities)
        {
            return await _repository.AddAllAsync(entities);
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _repository.UpdateAsync(entity);
        }
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _repository.DeleteAsync(entity);
        }
        public async Task<bool> DeleteAllAsync(List<TEntity> entities)
        {
            return await _repository.DeleteAllAsync(entities);
        }
    }
}
