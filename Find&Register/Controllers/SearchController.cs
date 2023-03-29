﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Find_Register.Filters;
using Microsoft.AspNetCore.Mvc;
using Find_Register.DataSourceService;
using Find_Register.Models;
using Find_Register.Cookies;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Logging;

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