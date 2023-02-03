namespace Find_Register.DataSourceService;

public class HttpClientWrapper : IHttpClient
{
    private readonly HttpClient _client;
    private readonly ILogger? _logger;

    public HttpClientWrapper(ILogger? logger = null)
    {
        _logger = logger;
        _client = new HttpClient();
    }

    public void AddHeader(string headerKey, string headerValue)
    {
        _client.DefaultRequestHeaders.Add(headerKey, headerValue);
    }

    public T? GetFromJson<T>(string url)
    {
        var task = Task.Run<T?>(async () => await _client.GetFromJsonAsync<T>(url));
        return task.Result;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}