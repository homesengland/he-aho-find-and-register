using Find_Register.Models;
using System.Text.Json;

namespace Find_Register.DataSourceService;

public class ProviderDataSource : IProviderDataSource
{

    private const string ProviderPathConfigKey = "DataSources:Providers";

    public ProviderDataSource(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger>();
        try
        {

            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var locationFile = config[ProviderPathConfigKey];
            var jsonString = System.IO.File.ReadAllText(locationFile);
            Providers = JsonSerializer.Deserialize<ProviderFileModel>(jsonString)?.Providers;
        }
        catch (Exception ex)
        {
            if (ex is JsonException) logger?.Log(LogLevel.Error, "Provider Json file is not valid");
            else if (ex is IOException) logger?.Log(LogLevel.Error, "Provider Json file does not exist");
            else if (ex is MissingMemberException) logger?.Log(LogLevel.Error, "Failed to load configuration from Service provider");
            else if (ex is ArgumentNullException) logger?.Log(LogLevel.Error, "Failed to load Provider json file path from configuration");

            Providers = new List<ProviderModel>();
        }
    }

    public IEnumerable<ProviderModel>? Providers { get; }

    public IEnumerable<ProviderModel>? ProvidersActiveInLocalAuthority(string localAuthority)
    {
        return Providers?.Where(p => p.Locations != null && p.Locations.Contains(localAuthority)).OrderBy(p => p.Name);
    }
}