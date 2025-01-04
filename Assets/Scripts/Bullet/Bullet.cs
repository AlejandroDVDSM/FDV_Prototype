using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1.0f;

    private BulletSpawner _spawner;

    private void Awake()
    {
        _spawner = GetComponentInParent<BulletSpawner>();
    }

    private void Update()
    {
        transform.Translate(Vector3.left * (_moveSpeed * Time.deltaTime), Space.World);
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

    private void Reset()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}