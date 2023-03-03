using System.Globalization;

namespace Find_Register.Models;

public class ProvidersSharePointAccessConfiguration
{
    public string? MicrosoftInstance { get; set; }
    public string? GraphUrl { get; set; }
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? ProviderSite { get; set; }
    public string? ProvidersList { get; set; }
    public string? SharepointHost { get; set; }

    /// <summary>
    /// URL of the authority
    /// </summary>
    public string Authority
    {
        get
        {
            return String.Format(CultureInfo.InvariantCulture, MicrosoftInstance ?? string.Empty, TenantId);
        }
    }
}