using System;
using System.Collections.Generic;

public class FactoryPresenter : IDisposable
{
    private Factory _factory;
    private FactoryView _view;
    private ITimer _productionTimer;

    public FactoryPresenter(Factory factory, FactoryView view, ITimer productionTimer)
    {
        _factory = factory;
        _view = view;
        _productionTimer = productionTimer;
        _productionTimer.Tick += _factory.ProduceResource;
        _factory.ResourceChanged += OnResourceChanged;
    }

    public int FactoryId => _factory.Id;

    public void Dispose()
    {
        StopProduce();
        _productionTimer.Tick -= _factory.ProduceResource;
        _factory.ResourceChanged -= OnResourceChanged;
    }

    public void StartProduce()
    {
        if (_productionTimer.IsRunning)
            return;

        _productionTimer.Start(_factory.ProduceInterval);
    }

    public void StopProduce()
    {
        _productionTimer.Stop();
    }

    public void SaveData()
    {
        ServiceLocator.Get<ISaveLoadService>().SaveFactory(_factory);
    }

    public KeyValuePair<ResourceType, int> TakeResources()
    {
        var result = _factory.TakeResources();
        return result;
    }

    private void OnResourceChanged(int amount)
    {
        SaveData();
        _view.RenderResourceAmount(amount);
    }
}