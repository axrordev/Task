using Microsoft.AspNetCore.Http;

namespace Task.Service.Helpers;

public static class HttpContextHelper
{
    public static IHttpContextAccessor ContextAccessor { get; set; }
    public static HttpContext HttpContext => ContextAccessor?.HttpContext;
    public static long GetUserId => Convert.ToInt64(HttpContext?.User.FindFirst("Id")?.Value);
}