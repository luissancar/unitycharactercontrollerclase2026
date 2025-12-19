using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")] public float moveSpeed = 5f;

    [Header("Salto / Gravedad")] public float jumpHeight = 3f;
    public float gravity = -9.81f;

    private CharacterController characterController;

    [SerializeField] private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested = false;

    [SerializeField] private AudioSource audioSourceSalto;
    [SerializeField] private AudioSource audioSourcePasos;
    [SerializeField] private int minSpeedSound = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController == null)
            return;
        ControlMovimiento();
        SonidoPasos();
    }

    private void SonidoPasos()
    {
        if (audioSourcePasos == null)
            return;
        Vector3 v = characterController.velocity;
        v.y = 0;
        bool andando = characterController.isGrounded && v.magnitude > minSpeedSound;
        if (andando)
        {
            if (!audioSourcePasos.isPlaying)
                audioSourcePasos.Play();
        }
        else if (audioSourcePasos.isPlaying)
            audioSourcePasos.Stop();
    }


    private void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpRequested = true;
    }

    private void ControlMovimiento()
    {
        bool isGrounded = characterController.isGrounded;
        //Reset vertical al tocar suelo
        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        //Movimiento local XZ
        Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);

        //convertir de local a mundo
        Vector3 worldMove = transform.TransformDirection(localMove);

        if (worldMove.sqrMagnitude > 1f)
            worldMove.Normalize();

        Vector3 horizontalVelocity = worldMove * moveSpeed;
        //Salto
        if (isGrounded && jumpRequested)
        {
            if (audioSourceSalto != null)
                audioSourceSalto.Play();
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }


        /////////Salto
        verticalVelocity += gravity * Time.deltaTime;
        //  Vector3 velocity = horizontalVelocity;
        // velocity.y = verticalVelocity;
        horizontalVelocity.y = verticalVelocity;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }
}