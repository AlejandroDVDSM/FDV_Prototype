using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;

    private BulletSpawner _spawner;
    private Vector3 _direction;
    
    private void Awake()
    {
        _spawner = GetComponentInParent<BulletSpawner>();
    }

    private void Update()
    {
        transform.Translate(_direction * (_moveSpeed * Time.deltaTime), Space.World);
    }

    private void OnDestroy()
    {
        _spawner.UnsubscribeBullet(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BulletLimit"))
            Reset();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Reset();
    }

    /// <summary>
    /// Set bullet movement direction
    /// </summary>
    /// <param name="direction">Direction of the movement</param>
    public void SetDirection(EBulletDirection direction)
    {
        switch (direction)
        {
            case EBulletDirection.LEFT:
                _direction = Vector3.left;
                break;
            case EBulletDirection.RIGHT:
                _direction = Vector3.right;
                break;
            case EBulletDirection.TOP:
                _direction = Vector3.up;
                break;
            case EBulletDirection.BOTTOM:
                _direction = Vector3.down;
                break;
        }
    }
    
    /// <summary>
    /// Disable the bullet and set its position back to the pool
    /// </summary>
    private void Reset()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}