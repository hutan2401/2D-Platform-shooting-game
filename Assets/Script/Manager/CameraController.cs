using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (cinemachineVirtualCamera == null)
        {
            Debug.LogWarning("Cinemachine Virtual Camera not found in the scene!");
            return;
        }

        if (PlayerControls.Instance == null)
        {
            Debug.LogWarning("PlayerControls instance not found!");
            return;
        }

        cinemachineVirtualCamera.Follow = PlayerControls.Instance.transform;
    }
}
