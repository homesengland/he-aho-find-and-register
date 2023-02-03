using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Find_Register.Models;

public class ProviderModel
{
    /// <summary>
    /// A Unique ID for this provider as stored in Homes England CRM system
    /// </summary>
    public string? CrmId { get; set; }

    /// <summary>
    /// Unique ID for this provider as sent through on Sharepoint list or other editing system
    /// </summary>
    public string? Id { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// Email for admin contact
    /// </summary>
    public string? AdminEmail { get; set; }

    /// <summary>
    /// Email for sales to display on search results
    /// </summary>
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Phone for sales to display on search results
    /// </summary>
    public string? Phone { get; set; }

    public string? Website { get; set; }

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

    /// <summary>
    /// Collection of Local authority names this provider operates in
    /// </summary>
    public List<string>? Locations {get; set;}
}

public class ProviderFileModel
{
    [JsonPropertyName("providers")]
    public List<ProviderModel>? Providers { get; set; }
}