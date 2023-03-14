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
        model.LocationModels = locations;
        model.ValidateLocalAuthorityAreaSearch(ModelState);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToAction("SearchResults", "Search", model.SimplifiedRedirectionModel());
       
    }

    [HttpGet]
    [HttpPost]
    [Route("organisations-that-sell-shared-ownership-homes")]
    public IActionResult SearchResults(SearchResultsModel model)
    {
        var locations = _locationDataSource.GetLocationDataSource.Locations;
        model.LocationModels = locations;
        model.ValidateLocalAuthorityAreaSearch(ModelState);

        if (!ModelState.IsValid)
        {
            return InvalidSearchResult(model);
        }

        var matchedLocation = locations?.FirstOrDefault(l => l.LocalAuthority?.Equals(model.Area ?? string.Empty) ?? false);
        if (matchedLocation?.IsLondon ?? false) { return SearchResultsLondon(model); }
        var gssCode = matchedLocation?.LocationCode;

        var providers = gssCode != null ?
            _locationDataSource.GetProviderDataSource.ProvidersActiveInLocalAuthority(gssCode) : new List<ProviderModel>();
        model.LocalAuthority = providers?.FirstOrDefault(p => p.IsLocalAuthority);
        model.ProviderModels = providers?.Where(p => !p.IsLocalAuthority);        

        if ((model.ProviderModels?.Count() ?? 0)== 0)
        {
            return NoSearchResults(model);
        }

        return View(model);
    }

    private IActionResult InvalidSearchResult(SearchResultsModel model)
    {
        return View(nameof(Index), model);
    }

    private IActionResult NoSearchResults(SearchResultsModel model)
    {
        return View(nameof(NoSearchResults), model);
    }

    private IActionResult SearchResultsLondon(SearchResultsModel model)
    {
        return View(nameof(SearchResultsLondon), model);
    }
}