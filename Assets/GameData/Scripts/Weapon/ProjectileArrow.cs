using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class ProjectileArrow : MonoBehaviour
{
    private bool _isEndLifepath = true;
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
        gameObject.transform.localScale = Vector3.zero;
        LMotion.Create(transform.localScale, scale, Random.Range(0.3f, 0.6f)).WithEase(Ease.InOutElastic).WithOnComplete(OnReadyState).BindToLocalScale(transform);
        gameObject.transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    public bool MoveForward(float _projectileSpeed)
    {
        gameObject.transform.position += transform.forward * _projectileSpeed * Time.deltaTime;
        return gameObject.activeSelf;
    }
    public void MoveDownward(float _projectileSpeed)
    {
        if (_isCanMove)
        {
            _isMoveNow = true;
            _isCanMove = false;

            if (transform.position.y > 0)
                LMotion.Create(transform.position.y, 0, _projectileSpeed).WithEase(Ease.InQuart).WithOnComplete(OnMoveState)
                .BindToPositionY(transform);
            else
                LMotion.Create(transform.localScale, Vector3.zero, 0.2f).WithEase(Ease.InOutElastic)
                .WithOnComplete(OnDeactivate).BindToLocalScale(transform);
        }
    }

    public void OnDeactivate()
    {
        gameObject.SetActive(false);
        _isEndLifepath = true;
        _isMoveNow = false;
    }

    public void OnMoveState() =>
        _isCanMove = !_isCanMove;
    public void OnReadyState() =>
        _isCanMove = !_isCanMove;
}
