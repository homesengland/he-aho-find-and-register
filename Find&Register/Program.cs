using Find_Register.Filters;
using Microsoft.ApplicationInsights.Extensibility;
using Find_Register.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using Find_Register.DataSourceService;
using Find_Register.Middleware;
using Find_Register.Models;
using Find_Register.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LocationConfiguration>(
    builder.Configuration.GetRequiredSection("DataSources:Locations")
);
builder.Services.Configure<ProvidersSharePointAccessConfiguration>(
    builder.Configuration.GetRequiredSection("SharePointGraph")
);

builder.Services.AddSingleton<IGraphServiceClientInstance, GraphServiceClientInstance>();
builder.Services.AddSingleton<IProviderDataSource, SharepointListProviderDataSource>();

builder.Services.AddAntiforgery();

builder.Services.AddSingleton<IDataSources, DataSources>();
builder.Services.AddSingleton<ISearchService, SearchService>();

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.Configure<TelemetryConfiguration>(
    x => x.DisableTelemetry = false
);
builder.Services.Configure<AnalyticsConfiguration>(
     builder.Configuration.GetRequiredSection("Analytics")
);

builder.Services.AddScoped<NonceModel>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews(config =>
    config.Filters.Add(typeof(UnhandledExceptionFilter)));
builder.Services.AddMvc(options => { options.Filters.Add(typeof(CustomAsyncResultFilterAttribute)); });
builder.Services.Configure<RazorViewEngineOptions>(o => o.ViewLocationExpanders.Add(
    new LibraryViewEngine()
));
builder.Services.AddScoped<ICookieHelper, CookieHelper>();
builder.Services.AddScoped<JourneyPageTrackerFilterAttribute>();
builder.Services.AddTransient<IHttpClient, HttpClientWrapper>();

var app = builder.Build();
app.UseHeaderSecurity();
app.UseCrossSiteScriptingSecurity();

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        Secure = CookieSecurePolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.Strict
    });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/GenericErrors/PageNotFound");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(ep => ep.MapRazorPages());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Eligibility}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    await next.Invoke();

    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
    {
         context.Response.Redirect("/GenericErrors/PageNotFound");
    }
    if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
    {
        context.Response.Redirect("/GenericErrors/InternalServerError");
    }
    if (context.Response.StatusCode == StatusCodes.Status503ServiceUnavailable)
    {
        context.Response.Redirect("/GenericErrors/ServiceUnavailable");
    }
    if (context.Response.StatusCode >= StatusCodes.Status400BadRequest)
    {
        context.Response.Redirect("/GenericErrors/InternalServerError");
    }
});

app.Run();