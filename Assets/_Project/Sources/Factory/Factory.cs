using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class Factory
{
    [JsonProperty] private int _id;
    [JsonProperty] private int _produceAmount;
    [JsonProperty] private int _resourceAmount;
    [JsonProperty] private float _produceInterval;
    [JsonProperty] private ResourceType _resourceType;

    public Factory(
        int id,
        int produceAmount,
        ResourceType resourceType,
        float produceInterval,
        int savedResourcesAmount = default)
    {
        _id = id;
        _produceAmount = produceAmount;
        _resourceType = resourceType;
        _produceInterval = produceInterval;
        _resourceAmount = savedResourcesAmount;
    }

    public event Action<int> ResourceChanged;

    public int Id => _id;
    public int ResourceAmount => _produceAmount; 
    public ResourceType ResourceType => _resourceType;
    public float ProduceInterval => _produceInterval;

    public void ProduceResource()
    {
        _resourceAmount += _produceAmount;
        ResourceChanged?.Invoke(_resourceAmount);
    }

    public KeyValuePair<ResourceType, int> TakeResources()
    {
        var amount = _resourceAmount;
        _resourceAmount = 0;
        ResourceChanged?.Invoke(_resourceAmount);
        return new KeyValuePair<ResourceType, int>(_resourceType, amount);
    }
}