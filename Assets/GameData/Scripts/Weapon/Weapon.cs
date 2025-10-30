using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IUpdatable
{
    public enum UpgradeType {Damage, AttackSpeed, PorjectileSpeed, RangeZone, WeaponSize, WeaponCount};

    [Space()]
    [Header("WeaponStats")]
    [SerializeField] protected float _weaponDamage;
    [SerializeField] protected float _weaponAttackSpeed; // time between attack
    [SerializeField] protected float _weaponProjectileSpeed;
    [SerializeField] protected float _weaponRangeZone;
    [SerializeField] protected float _weaponSize;
    [SerializeField] protected int _weaponCount;

    [Space()]
    [Header("WeaponStepStats")]
    [SerializeField] protected float _stepUpgradeDamage;
    [SerializeField] protected float _stepUpgradeAttackSpeed;
    [SerializeField] protected float _stepUpgradeProjectileSpeed;
    [SerializeField] protected float _stepUpgradeRangeZone;
    [SerializeField] protected float _stepUpgradeSize;
    [SerializeField] protected int _stepWeaponCount;

    [Space()]
    [Header("WeaponMaxStats")]
    [SerializeField] protected float _maxDamage;
    [SerializeField] protected float _maxAttackSpeed;
    [SerializeField] protected float _maxProjectileSpeed;
    [SerializeField] protected float _maxRangeZone;
    [SerializeField] protected float _maxSize;
    [SerializeField] protected int _maxCount;

    public void Start()
    {
        EventController.eventController.onAddUpdatable?.Invoke(this);
        TakeWeapon();
    }

    public abstract void TakeWeapon();
    public abstract void WeaponBehaviour();
    public abstract void Projectile();

    public virtual void UpgradeDamage() =>
        _weaponDamage += _stepUpgradeDamage;
    public virtual void UpgradeAttackSpeed() =>
        _weaponAttackSpeed += _stepUpgradeAttackSpeed;
    public virtual void UpgradeProjectileSpeed() =>
        _weaponProjectileSpeed += _stepUpgradeProjectileSpeed;
    public virtual void UpgradeRangeZone() =>
        _weaponRangeZone += _stepUpgradeRangeZone;
    public virtual void UpgradeWeaponSize() =>
        _weaponSize += _stepUpgradeSize;
    public virtual void UpgradeCount() =>
        _weaponCount++;

    public void Updatable()
    {
        WeaponBehaviour();
        Projectile();
    }

    public void UpgradeWeapon(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.Damage && _weaponDamage < _maxDamage)
            UpgradeDamage();
        else if (upgradeType == UpgradeType.AttackSpeed && _weaponAttackSpeed < _maxAttackSpeed)
            UpgradeAttackSpeed();
        else if (upgradeType == UpgradeType.PorjectileSpeed && _weaponProjectileSpeed < _maxProjectileSpeed)
            UpgradeProjectileSpeed();
        else if (upgradeType == UpgradeType.RangeZone && _weaponRangeZone < _maxRangeZone)
            UpgradeRangeZone();
        else if (upgradeType == UpgradeType.WeaponSize && _weaponSize < _maxSize)
            UpgradeWeaponSize();
        else if (upgradeType == UpgradeType.WeaponCount && _weaponCount < _maxCount)
            UpgradeCount();
    }

    public void OnEnable() =>
        EventController.eventController.onAddUpdatable?.Invoke(this);

    public void OnDisable() =>
        EventController.eventController.onRemoveUpdatable?.Invoke(this);

    [Button]
    public void UpgradeDamageDebug() =>
        UpgradeWeapon(UpgradeType.Damage);
    [Button]
    public void UpgradeAttackSpeedDebug() =>
        UpgradeWeapon(UpgradeType.AttackSpeed);
    [Button]
    public void UpgradeProjectileSpeedDebug() =>
        UpgradeWeapon(UpgradeType.PorjectileSpeed);
    [Button]
    public void UpgradeRangeZoneDebug() =>
        UpgradeWeapon(UpgradeType.RangeZone);
    [Button]
    public void UpgradeWeaponSizeDebug() =>
        UpgradeWeapon(UpgradeType.WeaponSize);
    [Button]
    public void UpgradeWeaponCountDebug() =>
        UpgradeWeapon(UpgradeType.WeaponCount);
}
