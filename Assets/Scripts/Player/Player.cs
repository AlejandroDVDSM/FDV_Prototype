using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int _currentHealth;
    private int _score;

    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    
    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
        
        _currentHealth = maxHealth;
        
        UIManager.Instance.UpdateHpTxt(_currentHealth);
        UIManager.Instance.UpdateScoreTxt(_score);
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UIManager.Instance.UpdateHpTxt(_currentHealth);
        _impulseSource.GenerateImpulse();
        _animator.SetTrigger("Hit");
        AudioManager.Instance.Play("Hit");
    }

    private void IncreaseScore(int score)
    {
        _score += score;
        UIManager.Instance.UpdateScoreTxt(_score);
        AudioManager.Instance.Play("Coin");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
            TakeDamage(10);

        if (other.gameObject.CompareTag("Bullet"))
            TakeDamage(25);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            IncreaseScore(10);
            Destroy(other.gameObject);
        }
    }
}
