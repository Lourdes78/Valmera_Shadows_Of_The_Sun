using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
            services[type] = service;
        else
            services.Add(type, service);
    }

    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.TryGetValue(type, out var service))
            return (T)service;

        throw new Exception($"Service of type {type} not registered.");
    }

    public static bool Exists<T>()
    {
        return services.ContainsKey(typeof(T));
    }

    public static void Clear()
    {
        services.Clear();
    }
}