using Microsoft.AspNetCore.Mvc;
using WatchCatalogAPI.Helper;
using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Controllers
{
    [Controller]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public JWTAuthModel auth => (JWTAuthModel)HttpContext.Items["JWTAuthModel"];
    }
}
