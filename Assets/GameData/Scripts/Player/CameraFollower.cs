using UnityEngine;

public class CameraFollower : MonoBehaviour, IUpdatable
{
    private Vector3 _offset;
    private Transform _playerMovement;
    private void Start()
    {
        EventController.eventController.onAddUpdatable?.Invoke(this);
        _playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<Transform>();
        _offset = transform.position;
    }

    public void Updatable() =>
        transform.position = Vector3.Lerp(transform.position, _playerMovement.position + _offset, 2 * Time.deltaTime);

    public void OnEnable() =>
        EventController.eventController.onAddUpdatable?.Invoke(this);
    public void OnDisable() =>
        EventController.eventController.onRemoveUpdatable?.Invoke(this);
}
