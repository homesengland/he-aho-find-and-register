using Find_Register.Extensions;
using Find_Register.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_Register.DataSourceService;

public class SearchService : ISearchService
{
    private readonly IDataSources _dataSources;
    private readonly ILogger<SearchService> _logger;

    public SearchService(IDataSources dataSources, ILogger<SearchService> logger)
    {
        _dataSources = dataSources;
        _logger = logger;
    }

    public SearchResultsModel InitializeSearchModel()
    {
        return new SearchResultsModel
        {
            LocationModels = _dataSources.GetLocationDataSource.Locations
        };
    }

    public SearchResultsModel ProcessSearch(SearchResultsModel model, ModelStateDictionary modelState)
    {
        var locations = _dataSources.GetLocationDataSource.Locations;
        model.LocationModels = locations;

        // here we build list of non-empty local authority inputs.
        var inputAreas = new List<string> { model.Area1, model.Area2, model.Area3 }
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .ToList();

        // then we validate that at least one authority was entered.
        if (!inputAreas.Any())
        {
            modelState.AddModelError(nameof(model.Area1), "Enter at least one local authority");
            return model;
        }

        // then we validate that each entered authority exists in the location data.
        var invalidAreas = inputAreas.Select((value, index) => new { index, value })
            .Where(x => !locations!.Any(l => l.LocalAuthority!.Equals(x.value, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var invalid in invalidAreas)
        {
            string propertyName = invalid.index switch
            {
                0 => nameof(model.Area1),
                1 => nameof(model.Area2),
                2 => nameof(model.Area3),
                _ => nameof(model.Area1)
            };
            string ordinal = invalid.index switch
            {
                0 => "1st",
                1 => "2nd",
                2 => "3rd",
                _ => ""
            };

            modelState.AddModelError(propertyName, $"Enter a valid local authority on {ordinal} input box");
        }
        if (!modelState.IsValid)
        {
            return model;
        }

        // so here we find matching locations, excluding London if needed.
        var matchedLocations = locations!
            .Where(l => inputAreas.Contains(l.LocalAuthority, StringComparer.OrdinalIgnoreCase) && !l.IsLondon)
            .ToList();

        // we get the unique location codes.
        var gssCodes = matchedLocations.Select(l => l.LocationCode).Distinct().ToList();

        // we aggregate provider data for each location code.
        var providers = new List<ProviderModel>();
        foreach (var code in gssCodes)
        {
            var localProviders = _dataSources.GetProviderDataSource.ProvidersActiveInLocalAuthority(code!);
            if (localProviders != null)
            {
                providers.AddRange(localProviders);
            }
        }

        // then we group providers into the appropriate categories.
        model.OpsoProviderModels = providers
            .Where(p => p.Opso && !p.IsLocalAuthority)
            .DistinctBy(p => p.Name);
        model.HoldProviderModels = providers
            .Where(p => p.Hold && !p.IsLocalAuthority)
            .Distinct();
        model.SharedOwnershipProviderModels = providers
            .Where(p => p.SharedOwnership && !p.IsLocalAuthority)
            .Distinct();
        model.RentToBuyProviderModels = providers
            .Where(p => p.RentToBuy && !p.IsLocalAuthority)
            .Distinct();

        // we populate local authority names for each provider.
        model.OpsoProviderModels.GetListOfLaNamesFromLocations(matchedLocations);
        model.HoldProviderModels.GetListOfLaNamesFromLocations(matchedLocations);
        model.SharedOwnershipProviderModels.GetListOfLaNamesFromLocations(matchedLocations);
        model.RentToBuyProviderModels.GetListOfLaNamesFromLocations(matchedLocations);

        var old_providers = new List<ProviderModel>(); // will be redundant -> after result PBI
        model.LocalAuthority = old_providers?.FirstOrDefault(p => p.IsLocalAuthority); // will be redundant -> after result PBI  
        model.ProviderModels = old_providers?.Where(p => !p.IsLocalAuthority); // will be redundant -> after result PBI  

        // Set local authority providers.
        model.LaModels = providers
            .Where(p => p.IsLocalAuthority)
            .DistinctBy(p => p.Name)
            .OrderBy(p => p.Name)
            .ToList();

        return model;
    }
}
