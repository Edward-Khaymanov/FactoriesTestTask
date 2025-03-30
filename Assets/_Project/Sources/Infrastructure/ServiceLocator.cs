using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private readonly Dictionary<Type, ILocatorService> _services;
    
    private ServiceLocator() 
    {
        _services = new Dictionary<Type, ILocatorService>();
    }

    public static ServiceLocator Current { get; private set; }

    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    public static T Get<T>() where T : ILocatorService
    {
        var key = typeof(T);
        if (Current._services.ContainsKey(key) == false)
        {
            Debug.LogError($"{key} not registered with {Current.GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)Current._services[key];
    }

    public static void Register<T>(T service) where T : ILocatorService
    {
        var key = typeof(T);
        if (Current._services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {Current.GetType().Name}.");
            return;
        }

        Current._services.Add(key, service);
    }

    public static void Unregister<T>() where T : ILocatorService
    {
        var key = typeof(T);
        if (Current._services.ContainsKey(key) == false)
        {
            Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {Current.GetType().Name}.");
            return;
        }

        Current._services.Remove(key);
    }
}