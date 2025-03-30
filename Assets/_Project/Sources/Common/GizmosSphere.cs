using UnityEngine;

public class GizmosSphere : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private Color _color = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _radius);
        Gizmos.color = Color.clear;
    }
}