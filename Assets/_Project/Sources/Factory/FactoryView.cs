using TMPro;
using UnityEngine;

public class FactoryView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;

    private ResourceType _resourceType;

    public void Init(ResourceType resourceType)
    {
        _resourceType = resourceType;
        RenderResourceAmount(0);
    }

    public void RenderResourceAmount(int amount)
    {
        _textComponent.text = $"{_resourceType} - {amount}";
    }
}