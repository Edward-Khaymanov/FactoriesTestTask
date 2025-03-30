using System.Collections.Generic;

public interface ISaveLoadService : ILocatorService
{
    public void Init();
    public void SaveFactory(Factory factory);
    public List<Factory> LoadFactories();
    public Dictionary<ResourceType, int> LoadResources();
    public void SaveResource(ResourceType resourceType, int amount);
}