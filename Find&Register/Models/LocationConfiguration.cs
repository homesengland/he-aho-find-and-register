namespace Find_Register.Models;

public class LocationConfiguration
{
    public bool UseApi { get; set; }
    public string LocalFile { get; set; } = "";
    public string ApiUrl { get; set; } = "";
    public string ApiClientName { get; set; } = "";
    public string ApiToken { get; set; } = "";
    public string TokenHeaderKey { get; set; } = "";
    public string ClientHeaderKey { get; set; } = "";
}