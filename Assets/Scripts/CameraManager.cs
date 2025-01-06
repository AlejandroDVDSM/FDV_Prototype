using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVCam;
    [SerializeField] private CinemachineVirtualCamera targetGroupVCam;

    public static CameraManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetPlayerVCamActive();
    }

    public void SetPlayerVCamActive()
    {
        playerVCam.gameObject.SetActive(true);
        targetGroupVCam.gameObject.SetActive(false);
    }

    public void SetTargetGroupVCamActive()
    {
     targetGroupVCam.gameObject.SetActive(true);
     playerVCam.gameObject.SetActive(false);
    }
}
