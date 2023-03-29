using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Find_Register.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Find_RegisterTest.DataSourceTests;

public class LocationApiTests
{
    private readonly LocationConfiguration _testConfig;

    public LocationApiTests()
    {
        var builder = WebApplication.CreateBuilder();
        ConfigurationManager configuration = builder.Configuration;
        configuration.AddUserSecrets<LocationApiTests>();
        _testConfig = configuration.GetSection("DataSources:Locations").Get<LocationConfiguration>();
    }


    [Fact]
    public void LocationApiDataSource_HasLondonBoolean()
    {
        var mockLogger = new MockLogger();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var client = new HttpClientWrapper(mockLogger);
        mockServiceProvider.Setup(m => m.GetService(typeof(IHttpClient))).Returns(client);

        var source = new LocationApiDataSource(_testConfig, null, mockServiceProvider.Object);
        Assert.Equal(309, source.Locations!.Count);
    }

    [Fact]
    public void LocationApiDataSource_UATResponseContainsLondonAreas()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogger = new MockLogger();
        var client = new HttpClientWrapper(mockLogger);
        mockServiceProvider.Setup(m => m.GetService(typeof(IHttpClient))).Returns(client);

        var source = new LocationApiDataSource(_testConfig, mockLogger, mockServiceProvider.Object);
        Assert.Equivalent(true, source.Locations!.Any(l => l.IsLondon));
    }

    [Fact]
    public void LocationApiDataSource_ContainsLondonAreasIfTheyAreReturnedFromApi()
    {
        var mockClient = new MockHttpClient();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogger = new MockLogger();
        mockServiceProvider.Setup(m => m.GetService(typeof(IHttpClient))).Returns(mockClient);
        var locationConfig = new LocationConfiguration
        {
            LocalFile = "Resources/TestLocations.json",
            UseApi = true
        };
        mockClient.SetReturnObject(() =>
        {
            return new LocationFileModel
            {
                LocalAuthorities = new List<LocationModel>
                {
                    new LocationModel{ LocalAuthority= "Central London", AreaCode="London Borough"},
                    new LocationModel{ LocalAuthority= "Milton Keynes", AreaCode="Buckinghamshire"},
                }
            };
        });
        var source = new LocationApiDataSource(locationConfig, mockLogger, mockServiceProvider.Object);
        Assert.Equivalent(true, source.Locations!.Any(l => l.IsLondon));
    }

    [Fact]
    public void LocationApiDataSource_HandlesErrors()
    {
        var mockClient = new MockHttpClient();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockLogger = new MockLogger();
        mockServiceProvider.Setup(m => m.GetService(typeof(IHttpClient))).Returns(mockClient);
        var locationConfig = new LocationConfiguration
        {
            LocalFile = "Resources/TestLocations.json",
            UseApi = true
        };
        mockClient.SetReturnObject(() => throw new HttpRequestException("501 or some other error"));

        var source = new LocationApiDataSource(locationConfig, mockLogger, mockServiceProvider.Object);
        Assert.Equivalent(0, source.Locations!.Count);
        Assert.Equivalent(1, mockLogger.LoggedMessages.Count);
    }
}

public class MockHttpClient : IHttpClient
{
    private Func<object>? _returnFunc;

    public void SetReturnObject(Func<object> returnFunc)
    {
        _returnFunc = returnFunc;
    }

    public T? GetFromJson<T>(string url)
    {
        return (T)(_returnFunc!.Invoke());
    }

    public void AddHeader(string headerKey, string headerValue)
    {
    }

    public void Dispose()
    {
    }
}