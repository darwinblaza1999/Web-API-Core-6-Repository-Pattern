using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.Data.SqlClient;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.ComponentModel;

namespace WatchCatalogAPI.Class
{
    public class Blob : IBlob
    {
        private readonly Connection _connection;
        private readonly string _secretValue;
        private readonly CloudBlobContainer _container;
        public Blob(Connection connection)
        {
            _connection = connection;
            _secretValue = _connection.CreateBlobStorage();
            CloudStorageAccount storage = CloudStorageAccount.Parse(_secretValue);
            CloudBlobClient blobClient = storage.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(_connection.Container);
        }
        public async Task<Response<object>> DeleteImageBlobStorage(string filename)
        {
            var response = new Response<object>();
            try
            {
                string blobName = new CloudBlockBlob(new Uri(_connection.CreateBlobUrl() + filename)).Name;
                var targetBlob = _container.GetBlockBlobReference(blobName);
                await targetBlob.DeleteIfExistsAsync();
                response.HttpCode = ResponseStatusCode.Success;
                response.DeveloperMessage = "Deleted";
                response.Code = ResponseCode.Success;
            }
            catch (Exception ex)    
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response<string>> UploadBlobStorage(IFormFile file)
        {
            var response = new Response<string>();
            try
            {
                using (Stream stream = file.OpenReadStream())
                {
                    await _container.CreateIfNotExistsAsync();
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var blob = _container.GetBlockBlobReference(imageName);
                    await blob.UploadFromStreamAsync(stream);
                    response.Data = blob.Uri.ToString();
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Code = ResponseCode.Success;
                    response.DeveloperMessage = "Successfully uploaded";
                }
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }
    }
}
