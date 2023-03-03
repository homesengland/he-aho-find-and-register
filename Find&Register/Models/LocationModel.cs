using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Find_Register.Models;

public class LocationModel
{
    private readonly static string[] LONDON_AREA_CODES = { "London Borough" };

    [JsonPropertyName("name")]
    public string? LocalAuthority { get; set; }

    [JsonPropertyName("gssCode")]
    public string? LocationCode { get; set; }

    [JsonPropertyName("areaCodeDescription")]
    public string? AreaCode { get; set; }

    public bool IsLondon => LONDON_AREA_CODES.Contains(AreaCode);
}

public class LocationFileModel
{
    [JsonPropertyName("localAuthorities")]
    public List<LocationModel>? LocalAuthorities { get; set; }
}
