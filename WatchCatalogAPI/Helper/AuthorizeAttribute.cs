using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute:Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = (JWTAuthModel)context.HttpContext.Items["JWTAuthModel"];

            var token_expired = (bool)context.HttpContext.Items["expired_token"];
            if (token_expired)
            {
                response.DeveloperMessage = "Expired Token";
                response.code = StatusCodes.Status401Unauthorized;
                // not logged in or role not authorized
                context.Result = new JsonResult(new { response }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (user == null)
            {
                response.DeveloperMessage = "Credential not validated";
                response.code = StatusCodes.Status401Unauthorized;
                // not logged in or role not authorized
                context.Result = new JsonResult(new { response }) { StatusCode = StatusCodes.Status401Unauthorized };

            }
        }
    }
}
