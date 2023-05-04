using WatchCatalogAPI.Repository;
using WatchCatalogAPI.Repository.Interface;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Class.Core
{
    public class Adapter : IAdapter

    {
        //public IGeneric<object> generic { get; }

        public IWatch watch { get; }
        public IAuthManager authManager { get; }

        public IBlob blob { get; }

        public Adapter(Connection con, IConfiguration config)
        {
            watch = new WatchRepository(con);
            authManager = new AuthManager(con, config);
            blob = new Blob(con); 
        }
    }
}
