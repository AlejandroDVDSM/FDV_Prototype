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

    /// <summary>
    /// Activate the player virtual camera at the same time that it deactivates other virtual cameras
    /// </summary>
    public void SetPlayerVCamActive()
    {
        playerVCam.gameObject.SetActive(true);
        targetGroupVCam.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activate the target group virtual camera at the same time that it deactivates other virtual cameras
    /// </summary>
    public void SetTargetGroupVCamActive()
    {
     targetGroupVCam.gameObject.SetActive(true);
     playerVCam.gameObject.SetActive(false);
    }
}
