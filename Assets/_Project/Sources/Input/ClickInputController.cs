using UnityEngine;
using UnityEngine.EventSystems;

public class ClickInputController : MonoBehaviour, IInputController
{
    private bool _enabled;
    private PlayerCamera _playerCamera;
    private Character _character;
    private Vector3 _lastMousePosition;
    private bool _isDragging;
    private bool _isHolding;
    private float _holdStartTime;
    private const float HOLD_THRESHOLD = 0.2f;

    public void Init(Character character, PlayerCamera playerCamera)
    {
        _playerCamera = playerCamera;
        _character = character;
    }

    private void Update()
    {
        if (_enabled == false)
            return;

        var isOverUi = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetKeyDown(KeyCode.Mouse0) && isOverUi == false)
        {
            _lastMousePosition = Input.mousePosition;
            _holdStartTime = Time.time;
            _isHolding = true;
            _isDragging = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_isHolding && Time.time - _holdStartTime > HOLD_THRESHOLD)
            {
                _isDragging = true;
                _isHolding = false;
            }

            if (_isDragging)
            {
                HandleDrag();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_isDragging == false)
                HandleClick();

            _isHolding = false;
            _isDragging = false;
        }
    }

    public void Enable()
    {
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
    }

    private void HandleClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        var mouseScreenPosition = Input.mousePosition;
        var ray = _playerCamera.Camera.ScreenPointToRay(mouseScreenPosition);
        var hit = Physics.Raycast(ray, out var hitInfo, 1000f, CONSTANTS.Layers.Factory | CONSTANTS.Layers.Terrain);

        if (hit == false)
            return;

        Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red, 1);
        if (hitInfo.transform.TryGetComponent<FactoryObject>(out var factoryObject))
        {
            _character.CollectResources(factoryObject);
            return;
        }

        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(nameof(CONSTANTS.Layers.Terrain)))
        {
            _character.MoveTo(hitInfo.point);
            return;
        }
    }

    private void HandleDrag()
    {
        var delta = Input.mousePosition - _lastMousePosition;
        _playerCamera.Move(new Vector2(delta.x, delta.y));
        _lastMousePosition = Input.mousePosition;
    }
}