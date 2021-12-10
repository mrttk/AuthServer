using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Responce<TDto>> GetByIdAsync(int id);
        Task<Responce<IEnumerable<TDto>>> GetAllAsync();
        Task<Responce<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Responce<TDto>> AddAsync(TEntity entity);
        Task<Responce<NoDataDto>> Remove(TEntity entity);
        Task<Responce<NoDataDto>> Update(TEntity entity);
    }
}
