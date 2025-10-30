using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform _backgroundJoystick;
    [SerializeField] private Transform _joystickKnot;
    private bool _isDraging = false;

    private float _maxKnotRadius = 120;

    private Vector3 _inputPosition;
    public Vector3 InputPosition { get { return _inputPosition; } }
    
    public void OnBeginDrag(PointerEventData handler)
    {
        _backgroundJoystick.gameObject.SetActive(true);
        _joystickKnot.gameObject.SetActive(true);
        _isDraging = true;

        _backgroundJoystick.position = handler.position;
        _joystickKnot.position = handler.position;
    }

    public void OnDrag(PointerEventData handler)
    {
        if (!_isDraging)
            return;

        Vector3 offset = handler.position - (Vector2)_backgroundJoystick.position;
        _inputPosition = new Vector3(offset.x, 0, offset.y).normalized;
        _joystickKnot.position = handler.position;
        if((_joystickKnot.position - _backgroundJoystick.position).sqrMagnitude > _maxKnotRadius * _maxKnotRadius)
            _joystickKnot.localPosition = Vector3.ClampMagnitude(_joystickKnot.localPosition, _maxKnotRadius);
    }
    
    public void OnEndDrag(PointerEventData handler)
    {
        _inputPosition = Vector3.zero;
        _backgroundJoystick.gameObject.SetActive(false);
        _joystickKnot.gameObject.SetActive(false);
        _isDraging = false;
    }
}
