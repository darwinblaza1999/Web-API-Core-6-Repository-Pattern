using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Repository.Interface
{
    public interface IWatch 
    {
        Task<Response<object>> AddWatch(WatchDetails model);
        Task<Response<object>> UpdateWatch(WatchDetails1 model);
        Task<Response<object>> Delete(int id);
        Task<Response<object>> GetWatchById(int id);
        Task<Response<List<object>>> GetAllAsync();
        Task<Response<object>> UpdateWatchImage(WatchImage model);
    }
}

