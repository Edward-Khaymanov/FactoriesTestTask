using UnityEngine;
using UnityEngine.EventSystems;

public class TouchInputController : MonoBehaviour, IInputController
{
    private bool _enabled;
    private PlayerCamera _playerCamera;
    private Character _character;
    private Vector2 _lastTouchPosition;
    private bool _isDragging;
    private bool _isHolding;
    private bool _isTouchOverUI;
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

        if (Input.touchCount != 1)
            return;

        var touch = Input.GetTouch(0);

        bool isOverUi = EventSystem.current.IsPointerOverGameObject(touch.fingerId);

        if (touch.phase == TouchPhase.Began)
        {
            _isTouchOverUI = isOverUi;
            _lastTouchPosition = touch.position;
            _holdStartTime = Time.time;
            _isHolding = true;
            _isDragging = false;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            if (_isTouchOverUI == false)
            {
                if (_isHolding && Time.time - _holdStartTime > HOLD_THRESHOLD)
                {
                    _isDragging = true;
                    _isHolding = false;
                }

                if (_isDragging)
                {
                    HandleDrag(touch.position);
                }
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            if (_isTouchOverUI == false && _isDragging == false)
                HandleTap(touch.position);

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

    private void HandleTap(Vector2 touchPosition)
    {
        if (_isTouchOverUI)
            return;

        var ray = _playerCamera.Camera.ScreenPointToRay(touchPosition);
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

    private void HandleDrag(Vector2 touchPosition)
    {
        var delta = touchPosition - _lastTouchPosition;
        _playerCamera.Move(delta);
        _lastTouchPosition = touchPosition;
    }
}
