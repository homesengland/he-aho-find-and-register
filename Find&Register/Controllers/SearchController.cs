using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Find_Register.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Find_Register.DataSourceService;
using Find_Register.Models;


namespace Find_Register.Controllers;
    
    [TypeFilter(typeof(UnhandledExceptionFilter))]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IDataSources _locationDataSource;

        // GET: /<controller>/
        public SearchController(ILogger<SearchController> logger, IDataSources locationDataSource)
        {
            _logger = logger;
            _locationDataSource = locationDataSource;
        }

        public IActionResult Index()
        {
            return View(new SearchResultsModel());
        }

        [HttpPost]
        public IActionResult SearchResults(SearchResultsModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }


            var providers = _locationDataSource.GetProviderDataSource.ProvidersActiveInLocalAuthority(model.Area ?? string.Empty);

            model.ProviderModels = providers;

            return View(model);
        }

        [HttpGet]
        public IActionResult SearchResults()
        {
            return View(new SearchResultsModel());
        }
    }
