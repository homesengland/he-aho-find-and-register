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
    public IActionResult Index(SearchResultsModel model)
    {
        var locations = _locationDataSource.GetLocationDataSource.Locations;
        if (!ModelState.IsValid)
        {
            return View(new SearchResultsModel { LocationModels = locations });
        }
        else
        {

            return RedirectToAction("SearchResults", "Search", model);
        }
       
    }

    [HttpGet]
    [HttpPost]
    [Route("organisations-that-sell-shared-ownership-homes")]
    public IActionResult SearchResults(SearchResultsModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var locations = _locationDataSource.GetLocationDataSource.Locations;
        var gssCode = locations?.FirstOrDefault(l => l.LocalAuthority?.Equals(model.Area ?? string.Empty) ?? false)?.LocationCode;

        if (string.IsNullOrEmpty(gssCode) && !(locations?.Any(l => l.LocationCode?.Equals(gssCode) ?? false) ?? false))
        {
            //add error message here
            return View(model);
        }

        var providers = _locationDataSource.GetProviderDataSource.ProvidersActiveInLocalAuthority(gssCode ?? string.Empty);

        var blobProviders = _locationDataSource.GetProviderBlobDataSource?.ProvidersActiveInLocalAuthority(gssCode ?? string.Empty);

        if (blobProviders != null)
        {
            foreach (var provider in blobProviders) {
                provider.Name = $"[Blob] {provider.Name}";
            }
            providers = providers?.Union(blobProviders);
        }

        model.ProviderModels = providers;
        model.LocationModels = locations;

        if (providers?.Count() == 0)
        {
            return NoSearchResults(model);
        }

        return View(model);
    }


    [HttpGet]
    [Route("organisations-that-sell-shared-no-results")]
    public IActionResult NoSearchResults()
    {
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Route("organisations-that-sell-shared-no-results")]
    public IActionResult NoSearchResults(SearchResultsModel model)
    {
        return View(nameof(NoSearchResults), model);
    }
}