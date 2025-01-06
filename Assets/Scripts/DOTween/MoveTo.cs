using System;
using DG.Tweening;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Start()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.DOMove(target.position, 10).SetEase(Ease.InOutSine);
    }
}
