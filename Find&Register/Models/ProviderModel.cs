using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Find_Register.DataSourceService;

namespace Find_Register.Models;

public class ProviderModel
{
    public ProviderModel(){
        Locations = new List<string>();
        LaLocations = new List<string>();
    }

    public ProviderModel(SharepointProviderValue providers) {
        Name = providers.CompanyName;
        Email = providers.Email;
        Phone = providers.ContactNumber;
        Website = providers.WebsiteName;
        WebsiteUrl = providers.WebsiteUrl;
        SharedOwnership = providers.SharedOwnership;
        Opso = providers.OPSO;
        Hold = providers.HOLD;
        RentToBuy = providers.RentToBuy;
        IsLocalAuthority = providers.IsLocalAuthority;
        Locations = new List<string>();
        Archived = providers.Archived;
        LaLocations = new List<string>();

        if (!string.IsNullOrWhiteSpace(providers.LocalAuthorities))
        {
            providers.LocalAuthorities?.Split(";")
                .ToList()
                .ForEach(la => Locations.Add(la));
        }
    }

    public string? Name { get; set; }

    /// <summary>
    /// Email for sales to display on search results
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone for sales to display on search results
    /// </summary>
    public string? Phone { get; set; }

    public string? Website { get; set; }
    public string? WebsiteUrl { get; set; }

    public bool SharedOwnership { get; set; }

    /// <summary>
    /// Indicates that this provider has OPSO properties
    /// </summary>
    [JsonPropertyName("OPSO")]
    public bool Opso { get; set; }

    /// <summary>
    /// Indicates that this provider has HOLD properties
    /// </summary>
    [JsonPropertyName("HOLD")]
    public bool Hold { get; set; }

    public bool RentToBuy { get; set; }

    public bool IsLocalAuthority { get; set; }    

    /// <summary>
    /// Collection of Local authority codes this provider operates in
    /// </summary>
    public List<string> Locations {get; set;}


    /// <summary>
    /// Indicates that this provider has been archived or still active
    /// </summary>
    public bool Archived { get; set; }

    /// <summary>
    /// Collection of Local authority names this provider operates in
    /// </summary>
    public List<string> LaLocations { get; set; }
}

public class ProviderFileModel
{
    [JsonPropertyName("providers")]
    public List<ProviderModel>? Providers { get; set; }
}

public class ProviderModelExtension : ProviderModel
{
    public List<string> AssociatedLocalAuthorites { get; set; } = new List<string>();
}