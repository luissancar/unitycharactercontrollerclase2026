using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Referencias")] public Transform cameraTransform;

    [Header("Mirar (ratón)")] public float mouseSensitivity = 120f;
    public float minPitch = -40f;
    public float maxPitch = 40f;

    private Vector2 lookInput;
    private float cameraPitch;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float yaw = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        cameraPitch = 0f;
        lookInput = Vector2.zero;
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.identity;
    }


    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTransform == null)
            return;
        HandleLook();
    }

    private void HandleLook()
    {
        throw new NotImplementedException();
    }
}