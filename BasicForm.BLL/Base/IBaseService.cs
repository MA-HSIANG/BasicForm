using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.BLL.Base
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<TEntity> GetByLongId(long id);
        Task<List<TEntity>> GetAllAsync();
        Task<bool> AddAsync(TEntity entity);
        Task<bool> AddAllAsync(List<TEntity> entities);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAllAsync(List<TEntity> entities);
    }
}
