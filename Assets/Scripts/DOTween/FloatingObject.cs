using System;
using DG.Tweening;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private void Start()
    {
        MakeObjectFloat();
    }

    private void MakeObjectFloat()
    {
        transform.DOMoveY(transform.position.y + .4f, 2).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
