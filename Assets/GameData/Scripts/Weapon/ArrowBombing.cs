using System.Collections.Generic;
using UnityEngine;

public class ArrowBombing : Weapon
{
    [SerializeField] private GameObject _prefabArrow;
    private List<ProjectileArrow> _arrowReady = new List<ProjectileArrow>();
    private List<ProjectileArrow> _arrowActive = new List<ProjectileArrow>();

    private int _takedArrowIndex = 0;
    private float _elapsedTime;

    public override void TakeWeapon()
    {
        float x;
        float z;

        Transform readyPrefab = Instantiate(_prefabArrow).GetComponent<Transform>();
        GameObject arrowReadyObject = new GameObject("BombingHolder");
        arrowReadyObject.AddComponent<ProjectileArrow>();

        for (int i = 0; i < 25; i++)
        {
            Transform prefab = Instantiate(readyPrefab);
            prefab.SetParent(arrowReadyObject.transform);
            x = Random.Range(arrowReadyObject.transform.position.x - _weaponSize,
            arrowReadyObject.transform.position.x + _weaponSize);
            z = Random.Range(arrowReadyObject.transform.position.z - _weaponSize,
            arrowReadyObject.transform.position.z + _weaponSize);
            prefab.position = new Vector3(x, arrowReadyObject.transform.position.y, z);
        }

        _arrowReady.Add(arrowReadyObject.GetComponent<ProjectileArrow>());

        _arrowReady[0].StateControll(false);
        Destroy(readyPrefab.gameObject);
    }
    public override void WeaponBehaviour()
    {
        _elapsedTime += _weaponAttackSpeed * Time.deltaTime;
        if (_elapsedTime > 2 && _arrowActive.Count == 0)
        {
            _takedArrowIndex++;
            if (_takedArrowIndex > _arrowActive.Count - 1)
                _takedArrowIndex = 0;

            foreach (var arrow in _arrowReady)
            {
                if (arrow.IsEndLifepath)
                {
                    _arrowActive.Add(arrow);

                    float x = Random.Range(transform.position.x - _weaponRangeZone,
                    transform.position.x + _weaponRangeZone);
                    float y = Random.Range(18f, 22f);
                    float z = Random.Range(transform.position.z - _weaponRangeZone,
                    transform.position.z + _weaponRangeZone);
                    float angle = Random.Range(0, 360);

                    Vector3 randomPosition = new Vector3(x, y, z);
                    arrow.SetTransform(randomPosition, Vector3.one, angle);
                    arrow.StateControll(true);
                }
            }

            _elapsedTime = 0;
        }
    }
    public override void Projectile()
    {
        if (_arrowActive.Count != 0 && _arrowActive[_takedArrowIndex].IsEndLifepath)
        {
            int lastIndex = _arrowActive.Count - 1;
            if (lastIndex == _takedArrowIndex)
                _arrowActive.RemoveAt(_takedArrowIndex);
            else
            {
                var lastElement = _arrowActive[lastIndex];
                _arrowActive[lastIndex] = _arrowActive[_takedArrowIndex];
                _arrowActive[_takedArrowIndex] = lastElement;
                _arrowActive.RemoveAt(lastIndex);
            }
        }

        foreach (var arrow in _arrowActive)
            arrow.MoveDownward(_weaponProjectileSpeed * Random.Range(1.2f, 2f));
    }

    public override void UpgradeCount()
    {
        //create part
        _weaponCount++;
        var instance = Instantiate(_arrowReady[0]);
        instance.StateControll(false);
        _arrowReady.Add(instance);

        //reset part
        foreach (var activeArrow in _arrowActive)
            activeArrow.OnDeactivate();

        _arrowActive.Clear();
        _elapsedTime = 0;
    }
}
