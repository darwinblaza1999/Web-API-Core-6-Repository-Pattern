using WatchCatalogAPI.Repository;
using WatchCatalogAPI.Repository.Interface;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Class.Core
{
    public class Adapter : IAdapter

    {
        //public IGeneric<object> generic { get; }

        public IWatch watch { get; }
        public Adapter(Connection con)
        {
            watch = new WatchRepository(con);
            //generic = new GenericRepository<object>(); 
        }
    }
}
