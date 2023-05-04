using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Repository.Interface
{
    public interface IAuthManager
    {
        ServiceResponse<JWTAuthModel> Auth(JWTAuthModel auth);
    }
}
