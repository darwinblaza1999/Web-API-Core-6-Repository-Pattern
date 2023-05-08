using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Repository.Interface
{
    public interface IGeneric<T> where T : class
    {
        Task<Response<T>> AddAsync(string sp, T model);
        Task<Response<T>> UpdateAsync(string sp, T model);
        Task<Response<T>> DeleteAsync(string sp, T id);
        Task<Response<List<T>>> GetAllAsync(string sp);
        Task<Response<T>> GetByIdAsync(string sp, T id);
    }
}
