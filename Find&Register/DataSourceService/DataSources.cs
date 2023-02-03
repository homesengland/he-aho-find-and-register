using System;
using Find_Register.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Find_Register.DataSourceService;

/// <summary>
/// DataSources service provides static data sources for non-transactional data.
/// 
/// To avoid loading and parsing data files for each request
/// we cache the loaded data for CachePeriodMinutes for all requests.
/// </summary>
public class DataSources : IDataSources
{
    private const uint CachePeriodMinutes = 5;    

    private readonly IMemoryCache _cache;
    private readonly IServiceProvider _serviceProvider;

    private const string LocationConfigKey = "DataSources:Locations";
    private readonly LocationConfiguration _locationConfig;
    
    private readonly ILogger? _logger;
    private readonly object _locationCacheKey = new object();
    private readonly object _providerCacheKey = new object();

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="cache"></param>    
    /// <param name="serviceProvider">resolve and store a reference to the dependency injector so that we can load registered data sources</param>
    public DataSources(IMemoryCache cache, IServiceProvider serviceProvider, IOptions<LocationConfiguration> config, ILogger? logger = null)
    {        
        _cache = cache;
        _serviceProvider = serviceProvider;
        _locationConfig = config.Value;
        _logger = logger;
    }

    public ILocationDataSource GetLocationDataSource
    {
        get
        {
            if (_cache.TryGetValue<ILocationDataSource>(_locationCacheKey, out var cachedLocationDataSource))
                return cachedLocationDataSource;

            ILocationDataSource locationDataSource = _locationConfig.UseApi ?
                new LocationApiDataSource(_locationConfig, _logger, _serviceProvider) :
                new LocationDataSource(_locationConfig, _logger);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachePeriodMinutes));
            _cache.Set<ILocationDataSource>(_locationCacheKey, locationDataSource, cacheEntryOptions);
            return locationDataSource;
        }
    }

    public IProviderDataSource GetProviderDataSource
    {
        get
        {
            var realDataSource = _serviceProvider.GetService<IProviderDataSource>();
            if (realDataSource != null) return realDataSource;

            if (_cache.TryGetValue<IProviderDataSource>(_providerCacheKey, out var cachedProviderDataSource))
                return cachedProviderDataSource;

            var providerDataSource = new ProviderDataSource(_serviceProvider);
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CachePeriodMinutes));
            _cache.Set<IProviderDataSource>(_providerCacheKey, providerDataSource, cacheEntryOptions);
            return providerDataSource;
        }
    }
}
