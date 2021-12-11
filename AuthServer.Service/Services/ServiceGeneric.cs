using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;
        public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Respose<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return Respose<TDto>.Success(newDto, 200);
        }

        public async Task<Respose<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            return Respose<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Respose<TDto>> GetByIdAsync(int id)
        {
            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null)
            {
                return Respose<TDto>.Fail("id not found!", 404, true);
            }
            return Respose<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
        }

        public async Task<Respose<NoDataDto>> Remove(int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Respose<NoDataDto>.Fail("Id not found", 404,true);
            }
            _genericRepository.Remove(isExistEntity);

            await _unitOfWork.CommitAsync();

            return Respose<NoDataDto>.Success(204);
        }

        public async Task<Respose<NoDataDto>> Update(TDto entity, int id)
        {
            var isExistEntitiy = await _genericRepository.GetByIdAsync(id);

            if (isExistEntitiy == null)
            {
                return Respose<NoDataDto>.Fail("Id not found!", 404, true);
            }

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            _genericRepository.Update(updateEntity);

            await _unitOfWork.CommitAsync();

            return Respose<NoDataDto>.Success(204);
        }

        public async Task<Respose<IEnumerable<TDto>>> Where(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);

            return Respose<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
        }
    }
}
