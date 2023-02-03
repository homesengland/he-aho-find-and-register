using System;
using Find_Register.Models;
using System.Collections.Generic;

namespace Find_Register.DataSourceService;

public interface IProviderDataSource
{
    public List<ProviderModel>? Providers { get; }

    public List<ProviderModel>? ProvidersActiveInLocalAuthority(string localAuthority);
}

