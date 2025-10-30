using System.Collections.Generic;
using UnityEngine;

public class StakesZone : Weapon
{
    [SerializeField] private GameObject _stakePrefab;
    private List<Transform> _readyStakes = new List<Transform>();
    private Transform _parentStakes;

    private Vector3 _startPosition;
    private float _elapsedTime;
    private float _attackPing = 2;

    public override void TakeWeapon()
    {
        _parentStakes = new GameObject().GetComponent<Transform>();
        _parentStakes.position = transform.position;
        Transform prefab = Instantiate(_stakePrefab).GetComponent<Transform>();
        for (int i = 0; i < 20; i++)
            _readyStakes.Add(Instantiate(prefab));

        Destroy(prefab.gameObject);
        RandomPositionStakes();

        foreach (var stake in _readyStakes)
            stake.SetParent(_parentStakes);

        _parentStakes.gameObject.SetActive(false);
    }
    public override void WeaponBehaviour()
    {
        _elapsedTime += _weaponAttackSpeed * Time.deltaTime;
        if (_elapsedTime > _attackPing)
        {
            if(!_parentStakes.gameObject.activeSelf)
            {
                _startPosition = transform.position;
                _parentStakes.position = _startPosition;
                _parentStakes.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                _parentStakes.gameObject.SetActive(true);
            }

            Vector3 targetPosition = _startPosition + Vector3.up * 3;
            _parentStakes.position = Vector3.MoveTowards(_parentStakes.position,
            targetPosition, _weaponAttackSpeed * 2 * Time.deltaTime);
            if (_parentStakes.position == targetPosition)
                _elapsedTime = 0;
        }
        else
        {
            Vector3 targetPosition = _startPosition + Vector3.down * 0;
            _parentStakes.position = Vector3.MoveTowards(_parentStakes.position,
            targetPosition, _weaponAttackSpeed * 2 * Time.deltaTime);
            if (_parentStakes.position == targetPosition)
                _parentStakes.gameObject.SetActive(false);
        }
    }
    public override void UpgradeCount()
    {
        Transform newStake = Instantiate(_readyStakes[0]);
        _readyStakes.Add(newStake);
        newStake.SetParent(_parentStakes);
        newStake.position = _readyStakes[0].position;
        RandomPositionStakes();
    }

    private void RandomPositionStakes()
    {
        float xRandom = 0;
        float zRandom = 0;
        foreach (var stake in _readyStakes)
        {
            xRandom = Random.Range(transform.position.x - _weaponRangeZone,
            transform.position.x + _weaponRangeZone);
            zRandom = Random.Range(transform.position.z - _weaponRangeZone,
            transform.position.z + _weaponRangeZone);
            Vector3 randomPosition = new Vector3(xRandom, stake.position.y, zRandom);
            stake.position = randomPosition;
        }
    }

    public override void Projectile()
    {
    }


}
