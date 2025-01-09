using System;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    [SerializeField] private GameObject invisibleWall;

    private void Start()
    {
        invisibleWall.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            EnableInvisibleWall();
    }

    /// <summary>
    /// Activates the invisible wall at the same time that it deactivates this same gameobject to avoid triggering it again
    /// </summary>
    public void EnableInvisibleWall()
    {
        invisibleWall.SetActive(true);
        gameObject.SetActive(false);
    }
}
