using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{
    public void StateControll(bool state) =>
        gameObject.SetActive(state);

    public void SetTransform(Vector3 position, Vector3 scale, float angleY)
    {
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        gameObject.transform.rotation = Quaternion.Euler(0, angleY, 0);
    }

    public bool Move(float _projectileSpeed)
    {
        gameObject.transform.position += transform.forward * _projectileSpeed * Time.deltaTime;
        return gameObject.activeSelf;
    }
}
