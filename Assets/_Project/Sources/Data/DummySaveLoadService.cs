using System.Collections.Generic;

public class DummySaveLoadService : ISaveLoadService
{
    private PlayerData _playerData;

    public void Init()
    {
        var produceInterval = 0.5f;
        _playerData = new PlayerData()
        {
            Factories = new List<Factory>()
            {
                new Factory(1, 1, ResourceType.Wood, produceInterval),
                new Factory(2, 2, ResourceType.Stone, produceInterval),
                new Factory(3, 3, ResourceType.Metal, produceInterval),
                new Factory(4, 4, ResourceType.Brick, produceInterval),
                new Factory(5, 5, ResourceType.Glass, produceInterval),
            },
            Resources = new Dictionary<ResourceType, int>()
            {
                [ResourceType.Wood] = 10,
                [ResourceType.Stone] = 20,
                [ResourceType.Metal] = 30,
                [ResourceType.Brick] = 40,
            }
        };
    }

    public void SaveFactory(Factory factory)
    {
        var savedFactoryIndex = _playerData.Factories.FindIndex(x => x.Id == factory.Id);
        if (savedFactoryIndex == -1)
            _playerData.Factories.Add(factory);
        else
            _playerData.Factories[savedFactoryIndex] = factory;
    }

    public List<Factory> LoadFactories()
    {
        return _playerData.Factories;
    }

    public Dictionary<ResourceType, int> LoadResources()
    {
        return _playerData.Resources;
    }

    public void SaveResource(ResourceType resourceType, int amount)
    {
        _playerData.Resources.TryAdd(resourceType, 0);
        _playerData.Resources[resourceType] = amount;
    }
}