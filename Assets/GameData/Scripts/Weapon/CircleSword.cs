using System.Collections.Generic;
using UnityEngine;

public class CircleSword : Weapon
{
    [SerializeField] private GameObject _weaponPreab;
    private List<Transform> _readyWeapons = new List<Transform>();

    private float _currentAngleOfSword;
    private float _angleStep;

    private float xCos;
    private float ySin;

    public override void TakeWeapon()
    {
        _readyWeapons.Add(Instantiate(_weaponPreab).GetComponent<Transform>());
    }

    public override void WeaponBehaviour()
    {
        _currentAngleOfSword += _weaponAttackSpeed * Time.deltaTime;
        if (_currentAngleOfSword > 360)
            _currentAngleOfSword = 0;

        _angleStep = 360 / _readyWeapons.Count;

        int swordIndex = 0;
        foreach (var sword in _readyWeapons)
        {

            float radAngle = _currentAngleOfSword + swordIndex * _angleStep;
            radAngle = radAngle * Mathf.Deg2Rad;

            xCos = Mathf.Cos(radAngle) * _weaponRangeZone;
            ySin = Mathf.Sin(radAngle) * _weaponRangeZone;

            Vector3 radialMovement = new Vector3(xCos, 1, ySin);
            sword.position = radialMovement + transform.position;

            Vector3 viewDirection = sword.position - transform.position;
            viewDirection.y = 0;
            sword.rotation = Quaternion.LookRotation(viewDirection);
            swordIndex++;
        }
    }

    public override void NewCountWeapon()
    {
        _weaponCount++;
        _readyWeapons.Add(Instantiate(_readyWeapons[0]));
    }

    public override void Projectile(){}
}
