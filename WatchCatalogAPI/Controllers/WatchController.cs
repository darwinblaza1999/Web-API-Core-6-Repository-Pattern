using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : ControllerBase
    {
        private readonly IAdapter _adapter;
        public WatchController(IAdapter adapter)
        {
            _adapter = adapter;
        }
        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> Add(WatchDetails details)
        {
            var response = await _adapter.watch.Add(details);
            return StatusCode((int)response.HttpCode, response);
        }
        [HttpPut]
        [Route("UpdateItem")]
        public async Task<IActionResult> Update(WatchDetails1 details)
        {
            var response = await _adapter.watch.Update(details);
            return StatusCode((int)response.HttpCode, response);
        }
        [HttpDelete]
        [Route("DeleteItem/{id}")]
        public async Task<IActionResult> Add(int id)
        {
            var response = await _adapter.watch.Delete(id);
            return StatusCode((int)response.HttpCode, response);
        }
        [HttpGet]
        [Route("GetAllItem")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _adapter.watch.GetAllAsync();
            return StatusCode((int)response.HttpCode, response);
        }
        [HttpGet]
        [Route("GetItemById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _adapter.watch.GetAsync(id);
            return StatusCode((int)response.HttpCode, response);
        }
    }
}
