using Find_Register.Cookies;
using Find_Register.DataSourceService;
using Find_Register.Filters;
using Find_Register.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Find_Register.Controllers;

[TypeFilter(typeof(UnhandledExceptionFilter))]
[JourneyLayoutFilter(Journey.Search)]
[Route("find-organisations-selling-shared-ownership-homes")]
public class SearchController : BaseControllerWithShareStaticPages
{
    private readonly ILogger<SearchController> _logger;
    private readonly IDataSources _locationDataSource;
    private readonly IAntiforgery _antiforgery;

    // GET: /<controller>/
    public SearchController(ILogger<SearchController> logger, IDataSources locationDataSource, ICookieHelper cookieHelper, IAntiforgery antiforgery) : base(cookieHelper)
    {
        _logger = logger;
        _locationDataSource = locationDataSource;
        _antiforgery = antiforgery;
    }

    [HttpGet]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult Index()
    {
        var locations = _locationDataSource.GetLocationDataSource.Locations;

        return View(new SearchResultsModel { LocationModels = locations });
    }

    [HttpGet]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
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

        if (model!.Products != null && model!.Products.Any())
        { 
            var hasSharedOwnershipProviders = model.Products.Contains(nameof(ProviderModel.SharedOwnership));
            var hasHoldProviders = model.Products.Contains(nameof(ProviderModel.Hold));
            var hasOpsoProviders = model.Products.Contains(nameof(ProviderModel.Opso));
            var hasRentToBuyProviders = model.Products.Contains(nameof(ProviderModel.RentToBuy));

            var sharedOwnershipProviders = providers!.Where(p => hasSharedOwnershipProviders && p.SharedOwnership);
            var holdProviders = providers!.Where(p => hasHoldProviders && p.Hold);
            var opsoProviders = providers!.Where(p => hasOpsoProviders && p.Opso);
            var rentToBuyProviders = providers!.Where(p => hasRentToBuyProviders && p.RentToBuy);

            providers = sharedOwnershipProviders
                .Union(holdProviders)
                .Union(opsoProviders)
                .Union(rentToBuyProviders)
                .Distinct();
        }

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