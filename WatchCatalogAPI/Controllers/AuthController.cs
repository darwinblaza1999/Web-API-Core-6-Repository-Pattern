using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WatchCatalogAPI.DTO;
using WatchCatalogAPI.Helper;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.UnitofWork;

namespace WatchCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        public readonly IAdapter _adapter;
        public readonly IMapper _mapper;
        public AuthController(IAdapter adapter, IMapper mapper)
        {
            _adapter = adapter;
            _mapper = mapper;
        }

        [HttpPost("GetToken")]
        [AllowAnonymous]
        public IActionResult GetAccount(JWTAuthModel auth)
        {
            ResponseMessage<JWTAuthModel> result =  _mapper.Map<ResponseMessage<JWTAuthModel>>(_adapter.authManager.Auth(auth));
            if(result.code == 200)
                return Ok(result);
            return BadRequest(result);

        }
    }
}
