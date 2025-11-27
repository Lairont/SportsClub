using System;
using System.Collections.Generic;
using System.Diagnostics;
using SportClub_Bancu.Domain.Enum;

namespace SportClub_Bancu.Domain.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
    public interface IBaseResponse<T>
    {
        T Data { get; set; }
    }

    public enum StatusCode
    {
        OK = 200,
        BadRequest = 400,
        NotFound = 404,
        InternalError = 500,
        InternalServerError = 501,
    }
}
