using Find_Register.DataSourceService;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Find_Register.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Find_RegisterTest.DataSourceTests;

public class ProviderBlobDataSourceTests
{
    private IServiceProvider _provider;

    public ProviderBlobDataSourceTests()
    {
        var builder = WebApplication.CreateBuilder();
        var configuration = builder.Configuration;
        configuration.AddUserSecrets<ProviderBlobDataSourceTests>();
        _provider = builder.Services.BuildServiceProvider();
    }

    [Fact(Skip = "Data may not be compatible untill we determine whether we are using sharepoint or blob storage")]
    public void LoadingProviderDataSourceFromBlobIsSuccessful()
    {
        var mockLogger = new Mock<ILogger<ProviderBlobDataSource>>();
        var mockLogger2 = new Mock<ILogger<BlobContainerClientWrapper>>();
        var config = _provider.GetService<IConfiguration>();
        var source = new ProviderBlobDataSource(mockLogger.Object, config!, new BlobContainerClientWrapper(config!, mockLogger2.Object));
        Assert.Equal(1, source.Providers?.Count());
    }

    [Fact]
    public void UploadTriggersNewData()
    {
        var mockLogger = new Mock<ILogger<ProviderBlobDataSource>>();
        var mockBlobClient = new Mock<IBlobContainerClient>();
        mockBlobClient.Setup(client => client.GetBlobLastModified(It.IsAny<string>())).Returns(new DateTimeOffset(DateTime.MinValue));
        mockBlobClient.Setup(client => client.DownloadBlobAndReturnLastModiedDate(It.IsAny<string>(), It.IsAny<string>())).Returns(new DateTimeOffset(DateTime.MinValue));
        var source = new ProviderBlobDataSource(mockLogger.Object, _provider.GetService<IConfiguration>()!, mockBlobClient.Object);
        var loadProviders = source.Providers;

        mockBlobClient.Verify(client => client.GetBlobLastModified(It.IsAny<string>()), Times.Once);
        mockBlobClient.Verify(client => client.DownloadBlobAndReturnLastModiedDate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        // First time providers are fetched from blob storage, they are downloaded

        loadProviders = source.Providers;
        mockBlobClient.Verify(client => client.GetBlobLastModified(It.IsAny<string>()), Times.Exactly(2));
        mockBlobClient.Verify(client => client.DownloadBlobAndReturnLastModiedDate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        // subsequent provider requests are not downloaded

        mockBlobClient.Setup(client => client.GetBlobLastModified(It.IsAny<string>())).Returns(new DateTimeOffset(DateTime.Now));
        loadProviders = source.Providers;
        mockBlobClient.Verify(client => client.GetBlobLastModified(It.IsAny<string>()), Times.Exactly(3));
        mockBlobClient.Verify(client => client.DownloadBlobAndReturnLastModiedDate(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        // unless last modified date has changed
    }
}

