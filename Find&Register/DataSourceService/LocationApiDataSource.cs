using Find_Register.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Find_Register.DataSourceService;

public class LocationApiDataSource : ILocationDataSource
{
    public LocationApiDataSource(LocationConfiguration config, ILogger? logger, IServiceProvider serviceProvider)
    {
        Locations = GetLocationsFromApi(config, logger, serviceProvider);
    }

    public List<LocationModel>? Locations { get; }

    private static List<LocationModel>? GetLocationsFromApi(LocationConfiguration config, ILogger? logger, IServiceProvider serviceProvider)
    {
        using var httpClient = serviceProvider.GetRequiredService<IHttpClient>();

        httpClient.AddHeader(config.TokenHeaderKey, config.ApiToken ?? "");
        httpClient.AddHeader(config.ClientHeaderKey, config.ApiClientName ?? "");

        try
        {
            var results = httpClient.GetFromJson<LocationFileModel>(config.ApiUrl ?? "");
            return results?.LocalAuthorities ?? new List<LocationModel>();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException) logger?.Log(LogLevel.Error, $"Request to API errored {ex.Message}");
            return new List<LocationModel>();
        }
    }
}
