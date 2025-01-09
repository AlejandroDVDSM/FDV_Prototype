using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Transform playerStart;
    
    private int _currentHealth;
    private int _score;

    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    
    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
        UIManager.Instance.UpdateHpTxt(_currentHealth);
        UIManager.Instance.UpdateScoreTxt(_score);
        
        RestorePosition();
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        // If the player health is 0, start from the beginning
        if (_currentHealth <= 0)
        {
            _currentHealth = maxHealth;
            RestorePosition();
        }
        
        UIManager.Instance.UpdateHpTxt(_currentHealth);
        _impulseSource.GenerateImpulse(); // Make the camera shake
        _animator.SetTrigger("Hit");
        AudioManager.Instance.Play("Hit");
    }

    private void IncreaseScore(int score)
    {
        _score += score;
        UIManager.Instance.UpdateScoreTxt(_score);
        AudioManager.Instance.Play("Coin");
    }

    /// <summary>
    /// Set player position to the start
    /// </summary>
    private void RestorePosition()
    {
        transform.position = playerStart.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Take damage if the character collisions with an obstacle
        if (other.gameObject.CompareTag("Obstacle"))
            TakeDamage(1);

        // Take damage if the character collisions with a bullet
        if (other.gameObject.CompareTag("Bullet"))
            TakeDamage(2);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player hits a collectable increase the score and destroy it
        if (other.CompareTag("Collectable"))
        {
            IncreaseScore(1);
            Destroy(other.gameObject);
        }
        
        // If the player falls into the void, restore its position to the start
        if (other.CompareTag("OffLimit"))
            RestorePosition();

        // If the player hits the special door trigger set the target group camera active to focus both the player and the door. 
        // Enable the invisible wall as well so the player can't go back
        if (other.CompareTag("DoorTrigger"))
        {
            CameraManager.Instance.SetTargetGroupVCamActive();
            other.GetComponent<InvisibleWall>().EnableInvisibleWall();
        }
        
        // If the player hits the door trigger display the end panel
        if (other.CompareTag("Door"))
            UIManager.Instance.DisplayEndPanel();
    }
}
