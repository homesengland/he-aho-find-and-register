using System;
using System.Collections.Generic;
using Find_Register.Models;

namespace Find_Register.DataSourceService;

public interface ILocationDataSource
{
    public List<LocationModel>? Locations { get; }
}