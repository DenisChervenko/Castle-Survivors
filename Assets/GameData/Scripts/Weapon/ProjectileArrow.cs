using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    private float _timeBeforeDelete = 0.3f;
    private float _elapsedTime;

    public void StateControll(bool state) =>
        gameObject.SetActive(state);

    public void SetTransform(Vector3 position, Vector3 scale, float angleY)
    {
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        gameObject.transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    public bool MoveForward(float _projectileSpeed)
    {
        gameObject.transform.position += transform.forward * _projectileSpeed * Time.deltaTime;
        return gameObject.activeSelf;
    }
    public bool MoveDownward(float _projectileSpeed)
    {
        if (transform.position.y > 0)
            gameObject.transform.position += Vector3.down * _projectileSpeed * Time.deltaTime;
        else
        {
            _elapsedTime += Time.deltaTime;
            if(_elapsedTime > _timeBeforeDelete)
            {
                _elapsedTime = 0;
                gameObject.SetActive(false);
            }
        }

        return gameObject.activeSelf;
    }
}
