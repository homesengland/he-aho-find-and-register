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
builder.Services.AddSingleton<IDataSources, DataSources>();

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
builder.Services.AddTransient<IHttpClient, HttpClientWrapper>();

var app = builder.Build();
app.UseHeaderSecurity();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/check-eligiblility-to-buy-a-shared-ownership-home/error");
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

app.Run();