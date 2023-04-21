using System.Data;
using System.Data.SqlClient;

namespace WatchCatalogAPI.Class
{
    public class Connection
    {
        public readonly IConfiguration config;
        public readonly string CatalogDB;
        public Connection(IConfiguration config)
        {
            this.config = config;
            this.CatalogDB = this.config.GetConnectionString("CatalogDB");
        }

        public IDbConnection CreateConnectionCatalogDB() => new SqlConnection(this.CatalogDB);
    }
}
