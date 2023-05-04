using Microsoft.AspNetCore.Mvc.Filters;

namespace WatchCatalogAPI.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =false, Inherited = true)]
    public class AllowAnonymousAttribute:Attribute, Microsoft.AspNetCore.Authorization.IAllowAnonymous
    {
        public void OuAuthorization(AuthorizationFilterContext context) { }

    }
}
