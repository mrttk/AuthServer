using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Respose<TEntity> where TEntity : class
    {
        public TEntity Data { get; private set; }
        public int StatusCode { get; set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }
        public ErrorDto Error { get; private set; }

        public static Respose<TEntity> Success(TEntity data, int statusCode)
        {
            return new Respose<TEntity> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Respose<TEntity> Success(int statusCode)
        {
            return new Respose<TEntity> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Respose<TEntity> Fail(ErrorDto errorDto, int statusCode)
        {
            return new Respose<TEntity>
            {
                Error = errorDto,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Respose<TEntity> Fail(string errorMessage, int statusCode, bool isShow)
        {
            var errorDto = new ErrorDto(errorMessage, isShow);

            return new Respose<TEntity> { Error = errorDto, StatusCode = statusCode, IsSuccessful = false };
        }


    }
}
