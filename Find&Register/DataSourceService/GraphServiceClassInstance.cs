using System.Net.Http.Headers;
using Find_Register.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Find_Register.DataSourceService;

public interface IGraphServiceClientInstance
{
    public DateTimeOffset? GetLastModified();
    public IEnumerable<ProviderModel> GetAllProviders();
}

public class GraphServiceClientInstance : IGraphServiceClientInstance
{
    private readonly ProvidersSharePointAccessConfiguration _config;
    private readonly ILogger _logger;

    public GraphServiceClientInstance(IOptions<ProvidersSharePointAccessConfiguration> config, ILogger<GraphServiceClientInstance> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public DateTimeOffset? GetLastModified()
    {
        try
        {
            var client = GetAuthenticatedGraphClient();

            var task = Task.Run<List>(async () => await client
                 .Sites
                .GetByPath($"sites/{_config.ProviderSite}", _config.SharepointHost)
                .Lists[_config.ProvidersList]
                .Request()
                .GetAsync());
            return task.Result.LastModifiedDateTime;
        }
        catch (Exception ex)
        {
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, $"Failed to fetch to Providers sharepoint list update time - {ex.Message}");
            return null;
        }        
    }

    public IEnumerable<ProviderModel> GetAllProviders()
    {
        var client = GetAuthenticatedGraphClient();
        IListItemsCollectionPage? sharePointData;

        try
        {
            var task = Task.Run<IListItemsCollectionPage>(async () => await client
                .Sites
                .GetByPath($"sites/{_config.ProviderSite}", _config.SharepointHost)
                .Lists[_config.ProvidersList]
                .Items
                .Request()
                .Expand(item => item.Fields)
                .Top(1000)
                .GetAsync());

            sharePointData = task.Result;
        }
        catch (Exception ex)
        {
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, $"Failed to connect to Providers sharepoint list - {ex.Message}");
            return new List<ProviderModel>();
        }

        var providerResult = new List<ProviderModel>();

        foreach (var dataItem in sharePointData)
        {
            try
            {
                var sharepointProvider = new SharepointProviderValue();
                sharepointProvider.SharedOwnership = Convert.ToBoolean(dataItem.Fields.AdditionalData["SharedOwnership"].ToString());
                sharepointProvider.CompanyName = dataItem.Fields.AdditionalData["CompanyName"].ToString();
                sharepointProvider.ContactNumber = GetOptionalValueFromSharepointDictionary(dataItem.Fields.AdditionalData, "ContactNumber");
                sharepointProvider.Email = dataItem.Fields.AdditionalData["Email"].ToString();
                sharepointProvider.WebsiteName = dataItem.Fields.AdditionalData["URL"].ToString();
                sharepointProvider.WebsiteUrl = CorrectUrl(dataItem.Fields.AdditionalData["URL"].ToString());
                sharepointProvider.HOLD = Convert.ToBoolean(dataItem.Fields.AdditionalData["HOLD"].ToString());
                sharepointProvider.IsLocalAuthority = Convert.ToBoolean(dataItem.Fields.AdditionalData["IsLocalAuthority"].ToString());
                sharepointProvider.LocalAuthorities = dataItem.Fields.AdditionalData["LocalAuthorities"].ToString();
                sharepointProvider.OPSO = Convert.ToBoolean(dataItem.Fields.AdditionalData["OPSO"].ToString());
                sharepointProvider.RentToBuy = Convert.ToBoolean(dataItem.Fields.AdditionalData["RentToBuy"].ToString());

                providerResult.Add(new ProviderModel(sharepointProvider));
            }
            catch (Exception ex)
            {
                // catch individual invalid rows and move on to the next row
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, $"Invalid sharepoint list value - {ex.Message}");
            }
        }
        return providerResult;
    }

    private static string? GetOptionalValueFromSharepointDictionary(IDictionary<string, object> collection, string key) {
        return collection.TryGetValue(key, out var value) ? value.ToString() : null;
    }

    private string CorrectUrl(string? originalUri)
    {
        if (string.IsNullOrEmpty(originalUri)) return "#";
        return Uri.IsWellFormedUriString(originalUri, UriKind.Absolute) ? originalUri : $"//{originalUri}";
    }

    private GraphServiceClient GetAuthenticatedGraphClient()
    {
        string[] scopes = new string[] { $"{_config.GraphUrl}.default" };
        // Generates a scope -> "https://graph.microsoft.com/.default"

        IConfidentialClientApplication app;
        {
            app = ConfidentialClientApplicationBuilder.Create(_config.ClientId)
                .WithClientSecret(_config.ClientSecret)
                .WithAuthority(new Uri(_config.Authority))
                .Build();
        }

        var graphServiceClient =
                new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
                {
                    AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.AccessToken);
                }));

        return graphServiceClient;
    }
}
