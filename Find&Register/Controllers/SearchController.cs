using Find_Register.Filters;
using Microsoft.AspNetCore.Mvc;
using Find_Register.DataSourceService;
using Find_Register.Models;
using Find_Register.Cookies;

namespace Find_Register.Controllers;

[TypeFilter(typeof(UnhandledExceptionFilter))]
[JourneyLayoutFilter(Journey.Search)]
[Route("find-organisations-selling-shared-ownership-homes")]
public class SearchController : BaseControllerWithShareStaticPages
{
    private readonly ILogger<SearchController> _logger;
    private readonly IDataSources _locationDataSource;

    // GET: /<controller>/
    public SearchController(ILogger<SearchController> logger, IDataSources locationDataSource, ICookieHelper cookieHelper) : base(cookieHelper)
    {
        _logger = logger;
        _locationDataSource = locationDataSource;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var locations = _locationDataSource.GetLocationDataSource.Locations;

        return View(new SearchResultsModel { LocationModels = locations });
    }

    [HttpPost]
    [Route("organisations-that-sell-shared-ownership-homes")]
    public IActionResult SearchResults(SearchResultsModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }


        var providers = _locationDataSource.GetProviderDataSource.ProvidersActiveInLocalAuthority(model.Area ?? string.Empty);
        var locations = _locationDataSource.GetLocationDataSource.Locations;

        model.ProviderModels = providers;
        model.LocationModels = locations;

        return View(model);
    }
}