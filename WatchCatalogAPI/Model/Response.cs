using Dapper;

namespace WatchCatalogAPI.Model
{
    public class Response<T>
    {
        public ResponseStatusCode HttpCode { get; set; }
        public string? DeveloperMessage { get; set; }
        public dynamic Code { get; set; } = ResponseCode.Default;
        public T? Data { get; set; }
    }
    public class Response2<T>:Response<T>
    {
        public string? UserMessage { get; set; }
        public DynamicParameters? param { get; set; }
        public string? token { get; set; }
    }
    public enum ResponseStatusCode
    {
        Success = 200,
        BadRequest = 400,
        InternalError = 500,
        Unauthorized = 401
    }
    public enum ResponseCode
    {
        Default = 0,
        Duplicate = 2,
        Success = 10,
        SPExecution = 80,
        NotFound = 90,
        SqlError = 91,
        InvalidFormat = 92,
        InternalException = 98,
        ProcessException = 99,
    }
    public class ServiceResponse<T>
    {
        public int code { get; set; }
        public string? DeveloperMessage { get; set; }
        public T? data { get; set; }
        public string? token { get; set; }
    }
}
