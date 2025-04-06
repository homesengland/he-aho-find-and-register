using Find_Register.Models;

namespace Find_Register.DataSourceService;

public class SharepointListProviderDataSource : IProviderDataSource
{
    private readonly ILogger<SharepointListProviderDataSource> _logger;
    private DateTimeOffset? _lastModified;
    private IEnumerable<ProviderModel> _providers = new List<ProviderModel>();
    private readonly IGraphServiceClientInstance _clientInstance;

    public SharepointListProviderDataSource(ILogger<SharepointListProviderDataSource> logger, IGraphServiceClientInstance clientInstance)
    {
        _logger = logger;
        _clientInstance = clientInstance;
    }

    public IEnumerable<ProviderModel> Providers
    {
        get
        {
            var lastModified = _clientInstance.GetLastModified();
            _logger.Log(LogLevel.Trace, $"Provider list updated {lastModified}, updating cached provider list");
            _lastModified = lastModified;
            _providers = _clientInstance.GetAllProviders();
            
            return _providers;
        }
    }

    public IEnumerable<ProviderModel> ProvidersActiveInLocalAuthority(string gssCode)
    {
        var tempProvider = Providers;
        _logger.Log(LogLevel.Trace, $"Searching for {gssCode} location in {tempProvider.Count()} providers");
        return tempProvider.Where(p => p.Locations.Contains(gssCode) && p.Archived == false).OrderBy(p => p.Name);
    }
}