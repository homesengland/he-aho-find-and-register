using Find_Register.Models;
using System.Text.Json;
using Azure.Storage.Blobs;

namespace Find_Register.DataSourceService;

public class ProviderBlobDataSource : IProviderDataSource
{

    private readonly string _connectionString;
    private readonly string _containerName;
    private const string _blobName = "SampleOutput.json";
    private const string _localFile = "providers.json";
    private DateTimeOffset? _lastModified;
    private readonly ILogger? _logger;


    public ProviderBlobDataSource(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetService<ILogger>();

        var config = serviceProvider.GetRequiredService<IConfiguration>();
        _connectionString = config["DataSources:Providers:ConnectionString"];
        _containerName = config["DataSources:Providers:ContainerName"];
    }

    private List<ProviderModel>? _providers;

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
        return Providers?.Where(p => p.Locations != null && p.Locations.Contains(localAuthorityGssCode)).OrderBy(p => p.Name);
    }

    private bool HasUpdates()
    {
        BlobContainerClient container = new BlobContainerClient(_connectionString, _containerName);
        BlobClient blob = container.GetBlobClient(_blobName);
        var modified = blob.GetProperties().Value.LastModified;
        return _lastModified == null || modified > _lastModified;
    }

    private void DownloadBlob()
    {
        BlobContainerClient container = new BlobContainerClient(_connectionString, _containerName);
        BlobClient blob = container.GetBlobClient(_blobName);
        var modified = blob.GetProperties().Value.LastModified;
        blob.DownloadTo(_localFile);
        _lastModified = modified;

        try
        {
            var jsonString = File.ReadAllText(_localFile);
            var sharePointProviders = JsonSerializer.Deserialize<SharepointProviderRoot>(jsonString)?.body?.value;
            _providers = sharePointProviders?.Select(p => new ProviderModel(p)).ToList() ?? new List<ProviderModel>();
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