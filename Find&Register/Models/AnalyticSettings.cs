namespace Find_Register.Models;

public struct AnalyticSettings
{
    public bool AcceptAnalytics { get; set; }
    public bool HideConfirmation { get; set; }
    public Guid? GoogleAnalyticsNoJsClientId { get; set; }
}
