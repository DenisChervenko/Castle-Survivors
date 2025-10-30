using System.Collections.Generic;
using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    public List<IUpdatable> _updateList = new List<IUpdatable>();
    public List<IUpdatable> _updatableForAdd = new List<IUpdatable>();
    public List<IUpdatable> _updatableForRemove = new List<IUpdatable>();
    private bool _isNeedChange = false;
    
    public void Update()
    {
        foreach (var update in _updateList)
            update.Updatable();

        if (_isNeedChange)
            OnChangeUpdate();
    }

    public void OnAddList(IUpdatable updatable)
    {
        _updatableForAdd.Add(updatable);
        _isNeedChange = true;
    }

    public void OnRemoveList(IUpdatable updatable)
    {
        _updatableForRemove.Add(updatable);
        _isNeedChange = true;
    }

    public void OnChangeUpdate()
    {
        foreach (var updatableRemove in _updatableForRemove)
            _updateList.Remove(updatableRemove);
        foreach (var updatableAdd in _updatableForAdd)
            _updateList.Add(updatableAdd);

        _updatableForRemove.Clear();
        _updatableForAdd.Clear();
        _isNeedChange = false;
    }

    public void OnEnable()
    {
        EventController.eventController.onAddUpdatable.AddListener(OnAddList);
        EventController.eventController.onRemoveUpdatable.AddListener(OnRemoveList);
    }
    public void OnDisable()
    {
        EventController.eventController.onAddUpdatable.RemoveListener(OnAddList);
        EventController.eventController.onRemoveUpdatable.RemoveListener(OnRemoveList);
    }
}
