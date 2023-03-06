using System.ComponentModel;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace Find_Register.DataSourceService;

public class BlobContainerClientWrapper : IBlobContainerClient
{
    private string _connectionString;
    private string _containerName;
    private ILogger? _logger;

    public BlobContainerClientWrapper(IConfiguration config, ILogger<BlobContainerClientWrapper> logger)
    {
        _connectionString = config["DataSources:Providers:ConnectionString"];
        _containerName = config["DataSources:Providers:ContainerName"];
        _logger = logger;
    }

    public DateTimeOffset? GetBlobLastModified(string blobName)
    {
        try
        {
            var client = new BlobContainerClient(_connectionString, _containerName);
            return client.GetBlobClient(blobName).GetProperties().Value.LastModified;
        }
        catch(Exception ex)
        {
            // e.g. access denied or blob not found
            _logger?.Log(LogLevel.Error, $"Request for blob errorred with {ex.Message}");            
            return null;
        }
    }

    public DateTimeOffset? DownloadBlobAndReturnLastModiedDate(string blobName, string localfile)
    {
        try
        {
            var client = new BlobContainerClient(_connectionString, _containerName);
            var blob = client.GetBlobClient(blobName);
            var modified = client.GetProperties().Value.LastModified;
            blob.DownloadTo(localfile);
            return modified;
        }
        catch (Exception ex)
        {
            // e.g. access denied or blob not found
            _logger?.Log(LogLevel.Error, $"Request for blob errorred with {ex.Message}");
            return null;
        }
    }
}