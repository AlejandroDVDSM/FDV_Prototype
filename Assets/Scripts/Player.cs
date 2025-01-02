using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMP_Text currentHealthTxt;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
        currentHealthTxt.text = $"HP: {_currentHealth}";
    }

    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        currentHealthTxt.text = $"HP: {_currentHealth}";
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
            TakeDamage(20);
    }
}
