using System.Data;
using System.Data.SqlClient;

namespace WatchCatalogAPI.Class
{
    public class Connection
    {
        public readonly IConfiguration config;
        public readonly string? CatalogDB;
        public readonly string? BlobStorage;
        public readonly string? Container;
        public readonly string? BlobUrl;
        public readonly string? AuthManagerKey;
        public readonly string? AuthManagerIssuer;
        public Connection(IConfiguration config)
        {
            this.config = config;
            this.CatalogDB = this.config["ConnectionStrings"];
            this.BlobStorage = this.config["BlobStorage"];
            this.Container = this.config["Container"];
            this.BlobUrl = this.config["BlobUrl"];
            this.AuthManagerKey = this.config["AuthManagerKey"];
            this.AuthManagerIssuer = this.config["AuthManagerIssuer"];
        }
            
        public IDbConnection CreateConnectionCatalogDB() => new SqlConnection(this.CatalogDB);
        public string CreateBlobStorage() => new string(this.BlobStorage);
        public string CreateContainer() => new string(this.Container);
        public string CreateBlobUrl() => new string(this.BlobUrl);
        public string CreateAuthKey() => new string(this.AuthManagerKey);
        public string CreateAuthIssuer() => new string(this.AuthManagerIssuer);
    }
}
