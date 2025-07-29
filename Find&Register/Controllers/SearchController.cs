using Find_Register.Cookies;
using Find_Register.DataSourceService;
using Find_Register.Filters;
using Find_Register.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using Find_Register.Extensions;

namespace Find_Register.Controllers;

[TypeFilter(typeof(UnhandledExceptionFilter))]
[JourneyLayoutFilter(Journey.Search)]
[Route("find-organisations-selling-shared-ownership-homes")]
public class SearchController : BaseControllerWithShareStaticPages
{
    private readonly ILogger<SearchController> _logger;
    private readonly ISearchService _searchService;
    private readonly IAntiforgery _antiforgery;

    // GET: /<controller>/
    public SearchController(ILogger<SearchController> logger, ISearchService searchService, ICookieHelper cookieHelper, IAntiforgery antiforgery) : base(cookieHelper)
    {
        _logger = logger;
        _searchService = searchService;
        _antiforgery = antiforgery;
    }

    [HttpGet]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    public IActionResult Index()
    {
        var model = _searchService.InitializeSearchModel();
        return View(model);
    }

    [HttpGet]
    [ServiceFilter(typeof(JourneyPageTrackerFilterAttribute))]
    [Route("organisations-that-sell-shared-ownership-homes")]
    public IActionResult SearchResults(SearchResultsModel model)
    {
        // Let's remove any binding errors for areas that may be revalidated by our new service/equest.
        ModelState.Remove(nameof(model.Area1));
        ModelState.Remove(nameof(model.Area2));
        ModelState.Remove(nameof(model.Area3));

        model = _searchService.ProcessSearch(model, ModelState);

        if (!ModelState.IsValid)
        {
            return InvalidSearchResult(model);
        }

        return View(model);
    }

    private IActionResult InvalidSearchResult(SearchResultsModel model)
    {
        return View(nameof(Index), model);
    }

    private IActionResult SearchResultsLondon(SearchResultsModel model)
    {
        return View(nameof(SearchResultsLondon), model);
    }
}