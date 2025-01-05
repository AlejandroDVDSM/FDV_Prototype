using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _spawnAmount = 3;
    [SerializeField] private float _repeatRate = 2.5f;
    
    [SerializeField] private EBulletDirection bulletDirection = EBulletDirection.LEFT;

    private List<Bullet> _bulletPool;
    private int _currentBulletIndex;
    
    private void Start()
    {
        InitBulletPool();
    }

    private void InitBulletPool()
    {
        _bulletPool = new List<Bullet>();

        for (int i = 0; i < _spawnAmount; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetDirection(bulletDirection);
            _bulletPool.Add(bullet);
            _bulletPool[i].gameObject.SetActive(false);
        }
        
        InvokeRepeating(nameof(SetPooledBulletActive), 0, _repeatRate);
    }

    private void SetPooledBulletActive()
    {
        
        if (_currentBulletIndex < _bulletPool.Count)
        {
            _bulletPool[_currentBulletIndex].gameObject.SetActive(true);
            
            _currentBulletIndex++;
        }
        else
            _currentBulletIndex = 0;
    }

    public void UnsubscribeBullet(Bullet bullet)
    {
        _bulletPool.Remove(bullet);
    }
}