using System;
namespace Find_Register.Extensions;

public static class ServiceProviderExtension
{
    public static T GetRequiredService<T>(this IServiceProvider provider)
    {
        var resolvedService = provider.GetService<T>();
        if (resolvedService == null) throw new MissingMemberException($"Required service of type {typeof(T)} was not found from Service Provider");
        return resolvedService;
    }
}