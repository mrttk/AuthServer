using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<TEntity> where TEntity : class
    {
        public TEntity Data { get; private set; }
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public ErrorDto Error { get; private set; }

        public static Response<TEntity> Success(TEntity data, int statusCode)
        {
            return new Response<TEntity> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<TEntity> Success(int statusCode)
        {
            return new Response<TEntity> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<TEntity> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Response<TEntity>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<TEntity> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);

            return new Response<TEntity> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }


    }
}
