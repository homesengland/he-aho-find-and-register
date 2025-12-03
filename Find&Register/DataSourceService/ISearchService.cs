using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_Register.DataSourceService;

public interface ISearchService
{
    /// <summary>
    /// Initializes the search model with common data (e.g. location list).
    /// </summary>
    SearchResultsModel InitializeSearchModel();

    /// <summary>
    /// Processes the search, validating inputs and populating provider results.
    /// </summary>
    SearchResultsModel ProcessSearch(SearchResultsModel model, ModelStateDictionary modelState);
}
