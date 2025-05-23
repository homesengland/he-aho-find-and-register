using Find_Register.DataSourceService;
using Find_Register.Models;

namespace Find_RegisterTest.DataSourceTests;

public class LocationDataSourceTests
{
    [Fact]
    public void LoadingLocationDataSourceFromFileIsSuccessful()
    {
        var locationConfig = new LocationConfiguration
        {
            LocalFile = "Resources/TestLocations.json",
            UseApi = false
        };        
        var source = new LocationDataSource(locationConfig, null);
        Assert.Equal(309, source.Locations!.Count);
    }

    [Fact]
    public void LoadingLocationDataSourceFromInvalidFileReturnsEmptyLocations()
    {
        var mockLogger = new MockLogger();
        var locationConfig = new LocationConfiguration
        {
            LocalFile = "Resources/Malformed.json",
            UseApi = false
        };

        var source = new LocationDataSource(locationConfig, mockLogger);
        Assert.Equivalent(0, source.Locations!.Count);
        Assert.Equivalent(1, mockLogger.LoggedMessages.Count);
    }
}

