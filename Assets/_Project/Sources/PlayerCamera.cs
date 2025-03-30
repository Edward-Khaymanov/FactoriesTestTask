using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed;

    public Camera Camera => _camera;

    public void Move(Vector2 direction)
    {
        var forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        var positionDelta = (transform.right * -direction.x + forward * -direction.y) * _moveSpeed;
        transform.position += positionDelta;
    }
}