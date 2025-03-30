using UnityEngine;

public class CollectPoint : MonoBehaviour
{
    [SerializeField] private float _radius;

    public bool CharacterInPoint { get; private set; }

    public bool ObjectInPoint(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= _radius;
    }
}