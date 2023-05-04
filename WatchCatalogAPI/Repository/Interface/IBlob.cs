using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Repository.Interface
{
    public interface IBlob
    {
        Task<Response<string>> UploadBlobStorage(IFormFile file);
        Task<Response<object>> DeleteImageBlobStorage(string url);
    }
}
