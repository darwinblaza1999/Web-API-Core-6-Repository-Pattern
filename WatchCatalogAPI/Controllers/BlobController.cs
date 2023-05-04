using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WatchCatalogAPI.DTO;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobController : BaseController
    {
        private readonly IAdapter _adapter;
        private readonly IMapper _mapper;
        public BlobController(IAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var response = await _adapter.blob.UploadBlobStorage(file);
            return StatusCode((int)response.HttpCode, response);
        }

        [HttpDelete]
        [Route("DeleteImage/{file}")]
        public async Task<IActionResult> DeleteImage(string file)
        {
            var response = await _adapter.blob.DeleteImageBlobStorage(file);
            return StatusCode((int)response.HttpCode, response);
        }
    }
}
