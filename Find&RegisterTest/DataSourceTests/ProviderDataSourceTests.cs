using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Find_Register.Models;

namespace Find_RegisterTest.DataSourceTests;

public class ProviderDataSourceTests
{
    [Fact]
    public void LoadingProviderDataSourceFromFileIsSuccessful()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockConfig = new MockConfig();
        mockConfig["DataSources:Providers"] = "Resources/TestProviders.json";
        mockServiceProvider.Setup(m => m.GetService(typeof(IConfiguration))).Returns(mockConfig);

        var source = new ProviderDataSource(mockServiceProvider.Object);
        Assert.Equal(280, source.Providers?.Count);
    }

    [Fact]
    public void ProvidersAtLocationFiltersToOnlyProvidersThatOperateInSpecifiedLocation()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockConfig = new MockConfig();
        mockConfig["DataSources:Providers"] = "Resources/TestProviders.json";
        mockServiceProvider.Setup(m => m.GetService(typeof(IConfiguration))).Returns(mockConfig);

        var locationConfig = new LocationConfiguration
        {
            LocalFile = "Resources/TestLocations.json",
            UseApi = false
        };
        var locations = new LocationDataSource(locationConfig, null).Locations;
        var locationName = locations?.Where(l => !l.IsLondon).First().LocalAuthority;


        var providerSource = new ProviderDataSource(mockServiceProvider.Object);
        var providersAtLocation = providerSource.ProvidersActiveInLocalAuthority(locationName!);
        Assert.Equal(5, providersAtLocation?.Count);
        // The dummy data has 1 provider for each location + 4 providers that operate nationally, thus this should always return 5 results

        foreach(var provider in providersAtLocation!)
        {
            Assert.Contains(locationName, provider.Locations!);
        }
    }

    [Fact]
    public void LoadingProviderDataSourceFromInvalidFileReturnsEmptyLocations()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockConfig = new MockConfig();
        var mockLogger = new MockLogger();
        mockConfig["DataSources:Providers"] = "Resources/Malformed.json";
        mockServiceProvider.Setup(m => m.GetService(typeof(IConfiguration))).Returns(mockConfig);
        mockServiceProvider.Setup(m => m.GetService(typeof(ILogger))).Returns(mockLogger);

        var source = new ProviderDataSource(mockServiceProvider.Object);
        Assert.Equal(0, source.Providers?.Count);
        Assert.Equal(1, mockLogger.LoggedMessages?.Count);

    }
}

