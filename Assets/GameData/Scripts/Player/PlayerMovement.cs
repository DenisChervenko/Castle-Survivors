using UnityEngine;

public class PlayerMovement : MonoBehaviour, IUpdatable
{
    [SerializeField] private float _speedMovement;
 
    private CharacterController _characterController;
    private Vector3 _movementDirection;
    private PlayerInput _playerInput;

    private Transform _camera;

    public void Start()
    {
        _camera = Camera.main.GetComponent<Transform>();

        EventController.eventController.onAddUpdatable?.Invoke(this);
        _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        _characterController = GetComponent < CharacterController>();
    }   

    public void Updatable()
    {
        if (_playerInput.InputPosition == Vector3.zero)
            return;

        Quaternion cameraDirection = Quaternion.Euler(0, _camera.eulerAngles.y, 0);
        Vector3 direction = cameraDirection * _playerInput.InputPosition;
        _movementDirection = direction * _speedMovement;
        _characterController.Move(_movementDirection * Time.deltaTime);
    }

    public void OnEnable() =>
        EventController.eventController.onAddUpdatable?.Invoke(this);
    public void OnDisable() =>
        EventController.eventController.onRemoveUpdatable?.Invoke(this);
}
