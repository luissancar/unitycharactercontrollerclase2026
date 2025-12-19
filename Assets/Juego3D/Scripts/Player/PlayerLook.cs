using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float delaySeconds = 2f;

    private Renderer[] renderers;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
        renderers = GetComponentsInChildren<Renderer>(true);
        Ocultar();
    }

    private void Ocultar()
    {
        foreach(var r in renderers) r.enabled = false;
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
        StartCoroutine("StartInput");
    }

    IEnumerator StartInput()
    {
        yield return new WaitForSeconds(delaySeconds);
        Mostrar();
        if (playerInput != null)
            playerInput.ActivateInput();
    }

    private void Mostrar()
    {
        foreach(var r in renderers) r.enabled = true;
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
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(0f, mouseX, 0f);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }
}