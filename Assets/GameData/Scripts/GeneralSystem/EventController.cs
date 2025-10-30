using UnityEngine;
using UnityEngine.Events;
public class EventController : MonoBehaviour
{
    public static EventController eventController;
    private void Awake() => eventController = this;

    public UnityEvent<IUpdatable> onAddUpdatable;
    public UnityEvent<IUpdatable> onRemoveUpdatable;
}
