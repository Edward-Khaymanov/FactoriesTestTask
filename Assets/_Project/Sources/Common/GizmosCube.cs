using UnityEngine;

public class GizmosCube : MonoBehaviour
{
    [SerializeField] private Vector3 _size = Vector3.one;
    [SerializeField] private Color _color = Color.red;

    private void OnDrawGizmos()
    {
        var originalMatrix = Gizmos.matrix;
        Gizmos.color = _color;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, _size);
        Gizmos.matrix = originalMatrix;
        Gizmos.color = Color.clear;
    }
}