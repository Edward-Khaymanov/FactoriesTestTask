using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageWindow : MonoBehaviour, ILocatorService
{
    [SerializeField] private ResourceAmountView _resourceAmountViewTemplate;
    [SerializeField] private Transform _resourceContainer;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _closeButton;

    private bool _isEnabled;
    private Dictionary<ResourceType, ResourceAmountView> _resourceTypeAmountViews;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Hide);
    }

    public void Init()
    {
        _resourceTypeAmountViews = new();
        var resourceTypes = ExtentionMethods.GetEnumValues<ResourceType>();

        foreach (var resourceType in resourceTypes)
        {
            var view = GameObject.Instantiate(_resourceAmountViewTemplate, _resourceContainer);
            _resourceTypeAmountViews.Add(resourceType, view);
        }
    }

    public void Show()
    {
        if (_isEnabled)
            return;

        var resources = ServiceLocator.Get<ISaveLoadService>().LoadResources();
        foreach (var item in _resourceTypeAmountViews)
        {
            resources.TryGetValue(item.Key, out var amount);
            item.Value.Render($"{item.Key} - {amount}");
        }
        gameObject.SetActive(true);
        _isEnabled = true;
    }

    public void Hide()
    {
        if (_isEnabled == false)
            return;

        gameObject.SetActive(false);
        _isEnabled = false;
    }
}