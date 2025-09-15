using System.Text.Json.Serialization;

public class SharepointProviderBody
{
    public List<SharepointProviderValue>? value { get; set; }
}
public class SharepointProviderRoot {
    public SharepointProviderBody? body { get; set; }
}
public class SharepointProviderValue
{
    public string? CompanyName { get; set; }
    public string? WebsiteName { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public bool SharedOwnership { get; set; }
    public bool IsLocalAuthority { get; set; }
    public bool OPSO { get; set; }
    public bool HOLD { get; set; }
    public bool RentToBuy { get; set; }
    public string? LocalAuthorities { get; set; }
}