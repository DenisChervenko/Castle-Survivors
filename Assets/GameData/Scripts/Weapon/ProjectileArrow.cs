using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class ProjectileArrow : MonoBehaviour
{
    private bool _isEndLifepath = false;
    private bool _isCanMove = false;
    private bool _isMoveNow = false;
    public bool IsMoveNow { get { return _isMoveNow; } }
    public bool IsEndLifepath { get { return _isEndLifepath; } }

    public void StateControll(bool state) =>
        gameObject.SetActive(state);

    public void SetTransform(Vector3 position, Vector3 scale, float angleY)
    {
        _isCanMove = false;
        _isEndLifepath = false;
        gameObject.transform.position = position;
        LMotion.Create(transform.localScale, scale, 0.5f).WithEase(Ease.InOutElastic).WithOnComplete(ReadyState).BindToLocalScale(transform);
        gameObject.transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    public void ReadyState() =>
        _isCanMove = !_isCanMove;

    public bool MoveForward(float _projectileSpeed)
    {
        gameObject.transform.position += transform.forward * _projectileSpeed * Time.deltaTime;
        return gameObject.activeSelf;
    }
    public bool MoveDownward(float _projectileSpeed)
    {
        if (_isCanMove)
        {
            if (transform.position.y > 0)
            {
                _isMoveNow = true;
                gameObject.transform.position += Vector3.down * _projectileSpeed * Time.deltaTime;
            }
            else
            {
                LMotion.Create(transform.localScale, Vector3.zero, 0.2f).WithEase(Ease.InOutElastic).WithOnComplete(OnDeactivate).BindToLocalScale(transform);
                _isCanMove = false;
            }
        }

        return gameObject.activeSelf;
    }
    
    public void OnDeactivate()
    {
        gameObject.SetActive(false);
        _isEndLifepath = true;
        _isMoveNow = false;
    }
}
