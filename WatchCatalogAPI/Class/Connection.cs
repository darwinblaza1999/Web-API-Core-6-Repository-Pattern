using System.Data;
using System.Data.SqlClient;

namespace WatchCatalogAPI.Class
{
    public class Connection
    {
        public readonly IConfiguration config;
        public readonly string CatalogDB;
        public readonly string BlobStorage;
        public readonly string Container;
        public readonly string BlobUrl;
        public Connection(IConfiguration config)
        {
            this.config = config;
            this.CatalogDB = this.config.GetConnectionString("CatalogDB");
            this.BlobStorage = this.config.GetConnectionString("BlobStorage");
            this.Container = this.config.GetConnectionString("Container");
            this.BlobUrl = this.config["AuthManager:BlobUrl"];
        }
            
        public IDbConnection CreateConnectionCatalogDB() => new SqlConnection(this.CatalogDB);
        public string CreateBlobStorage() => new string(this.BlobStorage);
        public string CreateContainer() => new string(this.Container);
        public string CreateBlobUrl() => new string(this.BlobUrl);
    }
}
