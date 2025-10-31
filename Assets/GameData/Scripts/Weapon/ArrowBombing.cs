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
        if(_elapsedTime > 2)
        {
            if( _arrowActive.Count != 0 &&_arrowActive[_takedArrowIndex].IsEndLifepath)
            {
                int lastIndex = _arrowActive.Count - 1;
                if(lastIndex == _takedArrowIndex)
                    _arrowActive.RemoveAt(_takedArrowIndex);
                else
                {
                    var lastElement = _arrowActive[lastIndex];
                    _arrowActive[lastIndex] = _arrowActive[_takedArrowIndex];
                    _arrowActive[_takedArrowIndex] = lastElement;
                    _arrowActive.RemoveAt(lastIndex);
                }
                
            }

            if(!_arrowActive[_takedArrowIndex].IsMoveNow)
            {
                _arrowActive.Add(_arrowReady[_takedArrowIndex]);

                float x = Random.Range(transform.position.x - _weaponRangeZone,
                transform.position.x + _weaponRangeZone);
                float z = Random.Range(transform.position.z - _weaponRangeZone,
                transform.position.z + _weaponRangeZone);
                float angle = Random.Range(0, 360);

                Vector3 randomPosition = new Vector3(x, 20, z);
                _arrowActive[_takedArrowIndex].SetTransform(randomPosition, Vector3.one, angle);
                _arrowActive[_takedArrowIndex].StateControll(true);

                _elapsedTime = 0;  
            }

            _takedArrowIndex++;
                if (_takedArrowIndex == _arrowReady.Count)
                    _takedArrowIndex = 0;
        }
    }
    public override void Projectile()
    {
        for(int i = _arrowActive.Count - 1; i >= 0; i--)
        {
            var arrow = _arrowActive[i];
            if(!arrow.MoveDownward(_weaponProjectileSpeed))
            {
                int last = _arrowActive.Count - 1;
                _arrowActive[i] = _arrowActive[last];
                _arrowActive.RemoveAt(last);
            }
        }
    }

    public override void UpgradeCount()
    {
        _weaponCount++;
        _arrowReady.Add(Instantiate(_arrowReady[0]));
    }
}
