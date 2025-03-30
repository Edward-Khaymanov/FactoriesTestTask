using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoriesManager : IDisposable, ILocatorService
{
    private FactoryView _factoryViewTemplate;
    private List<FactoryObject> _factoryObjects;

    public List<FactoryPresenter> FactoryPresenters { get; private set; }

    public void Init(FactoryView factoryViewTemplate, List<FactoryObject> factoryObjects)
    {
        FactoryPresenters = new List<FactoryPresenter>();
        _factoryViewTemplate = factoryViewTemplate;
        _factoryObjects = factoryObjects;

        var factories = ServiceLocator.Get<ISaveLoadService>().LoadFactories();
        foreach (var factory in factories)
        {
            var factoryObject = _factoryObjects.FirstOrDefault(x => x.FactoryId == factory.Id);
            if (factoryObject == null)
                continue;

            var factoryView = GameObject.Instantiate(_factoryViewTemplate, factoryObject.ResourceIndicatorPoint.position, Quaternion.Euler(CONSTANTS.CAMERA_ROTATION), factoryObject.transform);
            factoryView.Init(factory.ResourceType);

            var presenter = new FactoryPresenter(factory, factoryView, new UniTaskTimer());
            FactoryPresenters.Add(presenter);
        }
    }

    public void Dispose()
    {
        foreach (var presenter in FactoryPresenters)
        {
            presenter.Dispose();
        }
    }

    public void StartProduce()
    {
        foreach (var presenter in FactoryPresenters)
        {
            presenter.StartProduce();
        }
    }

    public KeyValuePair<ResourceType, int> TakeResources(int factoryId)
    {
        var factoryPresenter = FactoryPresenters.FirstOrDefault(x => x.FactoryId == factoryId);
        if (factoryPresenter == null)
            return default;

        return factoryPresenter.TakeResources();
    }
}