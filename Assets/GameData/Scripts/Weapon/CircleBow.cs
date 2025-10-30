using System.Collections.Generic;
using UnityEngine;

public class CircleBow : Weapon
{
    [SerializeField] private GameObject _bowPrefab;
    [SerializeField] private GameObject _projectilePrefab;
    private List<Transform> _readyBow = new List<Transform>();

    private List<ProjectileArrow> _readyProjectile = new List<ProjectileArrow>();
    private List<ProjectileArrow> _activeProjectile = new List<ProjectileArrow>();

    private int _takedArrowIndex = 0;
    private float _elapsedTime;
    private float _angleStep;
    private float _xCos;
    private float _zSin;

    public override void TakeWeapon()
    {
        _readyBow.Add(Instantiate(_bowPrefab).GetComponent<Transform>());
        ProjectileArrow _prefab = Instantiate(_projectilePrefab).GetComponent<ProjectileArrow>();
        for (int i = 0; i < 300; i++)
        {
            _readyProjectile.Add(Instantiate(_prefab));
            _readyProjectile[i].StateControll(false);
        }
            
    }

    public override void WeaponBehaviour()
    {
        _elapsedTime += _weaponAttackSpeed * Time.deltaTime;
        _angleStep = 360 / _readyBow.Count;

        int weaponIndex = 0;
        foreach (var bow in _readyBow)
        {
            float radialAngle = weaponIndex * _angleStep;
            radialAngle = radialAngle * Mathf.Deg2Rad;
            _xCos = Mathf.Cos(radialAngle) * _weaponRangeZone;
            _zSin = Mathf.Sin(radialAngle) * _weaponRangeZone;

            Vector3 StaticPosition = new Vector3(_xCos, 1, _zSin);
            bow.position = StaticPosition + transform.position;
            weaponIndex++;

            Vector3 lookDirection = bow.position - transform.position ;
            lookDirection.y = 0;
            bow.rotation = Quaternion.LookRotation(lookDirection);
        }
        
        if(_elapsedTime > 2)
        {
            foreach (var bow in _readyBow)
            {
                _activeProjectile.Add(_readyProjectile[_takedArrowIndex]);

                _activeProjectile[_takedArrowIndex].SetTransform(bow.position, Vector3.one * _weaponSize, bow.transform.eulerAngles.y);
                _activeProjectile[_takedArrowIndex].StateControll(true);

                _takedArrowIndex++;
                if (_takedArrowIndex > _activeProjectile.Count)
                    _takedArrowIndex = 0;
            }

            _elapsedTime = 0;
        }
    }

    public override void Projectile()
    {
        for(int i = _activeProjectile.Count - 1; i >= 0; i--)
        {
            var arrow = _activeProjectile[i];
            if(!arrow.Move(_weaponProjectileSpeed))
            {
                int last = _activeProjectile.Count - 1;
                _activeProjectile[i] = _activeProjectile[last];
                _activeProjectile.RemoveAt(last);
            }
        }
    }
    
    public override void UpgradeCount()
    {
        _weaponCount++;
        _readyBow.Add(Instantiate(_readyBow[0]));
    }
}
