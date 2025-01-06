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

    public void EnableInvisibleWall()
    {
        invisibleWall.SetActive(true);
        gameObject.SetActive(false);
    }
}
