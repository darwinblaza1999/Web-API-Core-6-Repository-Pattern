using Dapper;
using System.Data.SqlClient;
using System.Data;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WatchCatalogAPI.Repository;

namespace WatchCatalogAPI.Class
{
    public class WatchRepository : GenericRepository<object>, IWatch
    {
        private readonly Connection _connectionString;
        public WatchRepository(Connection connectionString):base(connectionString) => _connectionString = connectionString;

        public async Task<Response<object>> AddWatch(WatchDetails details)
        {
            return await AddAsync("usp_AddItem", details);
        }

        public async Task<Response<object>> Delete(int id)
        {
            var wid = new WatchId {itemNo = id};            
            return await DeleteAsync("usp_DeleteItem", wid);
        }

        public async Task<Response<List<object>>> GetAllAsync()
        {
            return await GetAllAsync("usp_GetAll");
        }

        public async Task<Response<object>> GetWatchById(int id)
        {
            var wid = new WatchId { itemNo = id, };
            return await GetByIdAsync("usp_GetById", wid);
        }

        public async Task<Response<object>> UpdateWatch(WatchDetails1 details)
        {
            var model = new WatchDetails1
            {
                ItemNo = details.ItemNo,
                ItemName = details.ItemName,
                ShortDescription = details.ShortDescription,
                FullDescription = details.FullDescription,
                Price = details.Price,
                Caliber = details.Caliber,
                Movement = details.Movement,
                Chronograph = details.Chronograph,
                Weight = details.Weight,
                Height = details.Height,
                Diameter = details.Diameter,
                Thickness = details.Thickness,
                Jewel = details.Jewel,
                CaseMaterial = details.CaseMaterial,
                StrapMaterial  = details.StrapMaterial,
            };
            return await UpdateAsync("usp_UpdateItem", model);
        }

        public async Task<Response<object>> UpdateWatchImage(WatchImage model)
        {
            return await UpdateAsync("usp_UpdateImage", model);
        }
    }
}
