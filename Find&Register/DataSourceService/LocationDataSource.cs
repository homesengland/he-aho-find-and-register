using System;
using System.IO;
using Find_Register.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Find_Register.DataSourceService;

public class LocationDataSource : ILocationDataSource
{
    public LocationDataSource(LocationConfiguration config, ILogger? logger)
    {
        try
        {
            var jsonString = File.ReadAllText(config.LocalFile);
            Locations = JsonSerializer.Deserialize<LocationFileModel>(jsonString)?.LocalAuthorities ?? new List<LocationModel>();
        }
        catch(Exception ex)
        {
            if (ex is JsonException) logger?.Log(LogLevel.Error, "Location Json file is not valid");
            else if (ex is IOException) logger?.Log(LogLevel.Error, "Location Json file does not exist");

            Locations = new List<LocationModel>();
        }
    }

    public List<LocationModel>? Locations { get; }
}

