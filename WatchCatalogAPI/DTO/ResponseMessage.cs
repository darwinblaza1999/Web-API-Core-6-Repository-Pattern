namespace WatchCatalogAPI.DTO
{
    public class ResponseMessage<T>
    {
        public int code { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }
        public string? token { get; set; }
    }
}
