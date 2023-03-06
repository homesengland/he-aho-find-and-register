using Find_Register.Models;
using System.Text.Json;

namespace Find_Register.DataSourceService;

public class ProviderBlobDataSource : IProviderBlobDataSource
{
    private const string _blobName = "SampleOutput.json";
    private const string _localFile = "providers.json";
    private DateTimeOffset? _lastModified;
    private readonly ILogger? _logger;
    private IBlobContainerClient _blobClient;


    public ProviderBlobDataSource(ILogger<ProviderBlobDataSource> logger, IConfiguration config, IBlobContainerClient blobClient)
    {
        _logger = logger;
        _blobClient = blobClient;
    }

    private List<ProviderModel>? _providers = new List<ProviderModel>();
    public IEnumerable<ProviderModel>? Providers
    {
        get
        {
            if (HasUpdates())
            {
                DownloadBlob();
            }
            return _providers;
        }
    }

    public IEnumerable<ProviderModel>? ProvidersActiveInLocalAuthority(string localAuthorityGssCode)
    {
        return Providers?.Where(p => p.Locations != null && p.Locations.Contains(localAuthorityGssCode)).OrderBy(p => p.Name).ToList();
    }

    private bool HasUpdates()
    {        
        var modified = _blobClient.GetBlobLastModified(_blobName);
        return modified != null && (_lastModified == null || modified > _lastModified);
    }

    private void DownloadBlob()
    {
        _lastModified = _blobClient.DownloadBlobAndReturnLastModiedDate(_blobName, _localFile);

        try
        {
            var jsonString = File.ReadAllText(_localFile);
            var sharePointProviders = JsonSerializer.Deserialize<SharepointProviderRoot>(jsonString)?.body?.value;
            _providers = sharePointProviders?.Select(p =>
            {
                return new ProviderModel(p);
            }).ToList() ?? new List<ProviderModel>();
        }
        catch (Exception ex)
        {
            if (ex is JsonException) _logger?.Log(LogLevel.Error, "Provider Json file is not valid");
            else if (ex is IOException) _logger?.Log(LogLevel.Error, "Provider Json file does not exist");
            else if (ex is MissingMemberException) _logger?.Log(LogLevel.Error, "Failed to load configuration from Service provider");
            else if (ex is ArgumentNullException) _logger?.Log(LogLevel.Error, "Failed to load Provider json file path from configuration");

            _providers = new List<ProviderModel>();
        }
    }
}