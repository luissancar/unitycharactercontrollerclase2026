using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")] [Tooltip("si se deja vacío, se buscarán automáticamente las camaras hijas.")] [SerializeField]
    private List<Camera> cameras = new List<Camera>();

    private int currentCamera = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (cameras == null || cameras.Count == 0)
        {
            cameras = new List<Camera>(GetComponentsInChildren<Camera>());
        }

        SetActiveCamera(currentCamera);
    }

    public void OnChangeCamera(InputValue value)
    {
        if (cameras == null || cameras.Count == 0)
            return;
        currentCamera++;
        if (currentCamera>=cameras.Count)
            currentCamera = 0;
        SetActiveCamera(currentCamera);
    }
    private void SetActiveCamera(int index)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            bool isActive = i == index;
            if (cameras[i] != null)
                cameras[i].enabled = isActive;
        }
    }
}