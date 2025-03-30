using UnityEngine;

public class FactoryObject : MonoBehaviour
{
    [SerializeField] private int _factoryId;
    [SerializeField] private CollectPoint _collectPoint;
    [SerializeField] private Transform _resourceIndicatorPoint;

    public int FactoryId => _factoryId;
    public CollectPoint CollectPoint => _collectPoint;
    public Transform ResourceIndicatorPoint => _resourceIndicatorPoint;
}