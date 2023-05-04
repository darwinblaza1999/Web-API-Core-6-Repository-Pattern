using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Repository.Interface
{
    public interface IWatch 
    {
        Task<Response<object>> Add(WatchDetails model);
        Task<Response<object>> Update(WatchDetails1 model);
        Task<Response<object>> Delete(int id);
        Task<Response<object>> GetAsync(int id);
        Task<Response<object>> GetAllAsync();
        Task<Response<object>> UpdateImage(WatchImage model);
    }
}

