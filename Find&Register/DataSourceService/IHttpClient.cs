namespace Find_Register.DataSourceService;

public interface IHttpClient : IDisposable
{
    void AddHeader(string headerKey, string headerValue);

    T? GetFromJson<T>(string url);
}
