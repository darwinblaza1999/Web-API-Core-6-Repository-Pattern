using System.ComponentModel;
using System.Net;
using System.Text.Json;
using WatchCatalogAPI.Helper;
using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {

            ServiceResponse<string> apiResponse = new ServiceResponse<string>();
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                apiResponse.code = response.StatusCode;
                apiResponse.DeveloperMessage = error?.Message + " -. " + error.StackTrace + " " + error.Source;

                var result = JsonSerializer.Serialize(apiResponse);
                await response.WriteAsync(result);
            }
        }

    }
}
