namespace Find_Register.DataSourceService;

public interface IBlobContainerClient
{
    public DateTimeOffset? GetBlobLastModified(string blobName);
    public DateTimeOffset? DownloadBlobAndReturnLastModiedDate(string blobName, string localfile);
}