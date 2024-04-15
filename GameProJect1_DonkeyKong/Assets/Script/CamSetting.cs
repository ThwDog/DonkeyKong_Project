using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CamSetting : MonoBehaviour
{
    public Transform follow;
    public Transform lookAt;
    
    [Header("CamSetting")]
    [SerializeField]public float fov = 60f;

    public CinemachineVirtualCamera cam;

    public bool allowRuntimeCameraSettingsChanges;

    private void Awake() 
    {
        UpdateCameraSettings();
        
        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null && player.CompareTag("Player"))
        {
            follow = player.transform;
            lookAt = follow;
        }
    }

    void Update()
    {
        if(allowRuntimeCameraSettingsChanges){
            UpdateCameraSettings();
        }
    }

    void UpdateCameraSettings()
    {

        cam.Follow = follow;
        cam.m_Lens.FieldOfView = fov;

        if(lookAt != null)
            cam.LookAt = lookAt;    
    }

}
