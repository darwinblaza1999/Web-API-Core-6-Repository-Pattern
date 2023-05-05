using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchCatalogAPI.DTO;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : BaseController
    {
        private readonly IAdapter _adapter;
        private readonly IMapper _mapper;
        public WatchController(IAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
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
        [Route("GetItemById/{ItemNo}")]
        public async Task<IActionResult> GetById(int ItemNo)
        {
            var response = await _adapter.watch.GetAsync(ItemNo);
            return StatusCode((int)response.HttpCode, response);
        }
        [HttpPut]
        [Route("UpdateImage")]
        public IActionResult UpdateImage(WatchImage model)
        {
            DTOResponse<DTOWatchModel> result = _mapper.Map<DTOResponse<DTOWatchModel>>(_adapter.watch.UpdateImage(model));
            return StatusCode((int)result.HttpCode, result);
        }   
    }
}
