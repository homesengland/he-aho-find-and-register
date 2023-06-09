using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_Register.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class CrossSiteScriptingMiddleware
{
  private readonly RequestDelegate _next;

  public CrossSiteScriptingMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context, NonceModel nonce)
  {
    StringBuilder contenSecurityPolicy = new StringBuilder();

    contenSecurityPolicy.Append("default-src 'self';");
    contenSecurityPolicy.Append($"script-src 'strict-dynamic' 'nonce-{nonce.GaNonce}' ");
    contenSecurityPolicy.Append("'self' https://www.google-analytics.com;");
    contenSecurityPolicy.Append("object-src 'none';");
    contenSecurityPolicy.Append("base-uri 'none';");
    contenSecurityPolicy.Append("referrer 'none'");

    const string csp = "Content-Security-Policy";
    if (context.Response.Headers.All(x => x.Key != csp))
    {
      context.Response.Headers.Add(csp, contenSecurityPolicy.ToString());
    }

    const string referrerPolicy = "Referrer-Policy";
    if (context.Response.Headers.All(x => x.Key != referrerPolicy))
    {
      context.Response.Headers.Add(referrerPolicy, "none");
    }

    await _next(context);
  }
}

public static partial class HeaderSecurityMiddlewareExtensions
{
  public static IApplicationBuilder UseCrossSiteScriptingSecurity(
      this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<CrossSiteScriptingMiddleware>();
  }
}