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
        Task<Respose<TDto>> GetByIdAsync(int id);
        Task<Respose<IEnumerable<TDto>>> GetAllAsync();
        Task<Respose<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Respose<TDto>> AddAsync(TDto entity);
        Task<Respose<NoDataDto>> Remove(int id);
        Task<Respose<NoDataDto>> Update(TDto entity, int id);
    }
}
