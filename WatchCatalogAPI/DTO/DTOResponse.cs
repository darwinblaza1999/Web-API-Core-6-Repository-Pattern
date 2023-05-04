using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.DTO
{
    public class DTOResponse<T>
    {
        public ResponseStatusCode HttpCode { get; set; }
        public string? DeveloperMessage { get; set; }
        public dynamic Code { get; set; } = ResponseCode.Default;
        public T? Data { get; set; }
    }
}
