
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")] public float moveSpeed = 5f;

    [Header("Salto / Gravedad")] public float jumpHeight = 3f;
    public float gravity = -9.81f;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested = false;

    [SerializeField] private AudioSource audioSourceSalto;
    [SerializeField] private AudioSource audioSourcePasos;
    [SerializeField] private int minSpeedSound = 1;

    [SerializeField] private Animator animator;
    
    bool isGrounded;

    private bool saltando = false;
    private float saltandoAnterior;

    [SerializeField] private HacerseHijoMano hacerseHijoMano;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
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
        ControlAnimacion();
        SonidoPasos();
    }


    private void OnSoltar(InputValue value)
    {
        hacerseHijoMano.SoltarPalo();
    }
    private void ControlAnimacion()
    {
        
        Vector3 velocidad = characterController.velocity;
        Vector3 movimientoLocal = transform.InverseTransformDirection(velocidad);
        
        animator.SetFloat("X", movimientoLocal.x);
        animator.SetFloat("Y", movimientoLocal.z);
        animator.SetBool("EnSuelo", isGrounded);
        animator.SetFloat("Z", verticalVelocity);
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
        if (value.isPressed) // Comprueba si el botón de salto está presionado.
            jumpRequested = true; // Marca que se ha solicitado un salto; se usará en el siguiente Update.
    }
/// <summary>
    private void ControlMovimiento()
    {
        // Movimiento horizontal (local -> world)
        Vector3 localMove = new Vector3(moveInput.x, 0f, moveInput.y);
        if (localMove.sqrMagnitude > 1f) localMove.Normalize();

        Vector3 worldMove = transform.TransformDirection(localMove);
        Vector3 velocity = worldMove * moveSpeed;

        // Si estamos en el suelo (del frame anterior) y cayendo, mantener pegado
        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        // Salto
        if (isGrounded && jumpRequested)
        {
            if (audioSourceSalto != null) audioSourceSalto.Play();
            animator.SetTrigger("Saltar");
            saltando = true;
            //saltandoAnterior = verticalVelocity-2;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
            isGrounded = false;
        }

     /*   if (saltando)
        {
            if (verticalVelocity < saltandoAnterior)
            {
                verticalVelocity = 0;
                saltando=false;
            }
            else
            {
                saltandoAnterior = verticalVelocity;
            }
        }
        
        
        Debug.Log($"gravity={gravity}  vY={verticalVelocity}  grounded={isGrounded}");
*/
        // Gravedad SIEMPRE (pero con el reset anterior ya no se irá a -50 estando en suelo real)
        verticalVelocity += gravity * Time.deltaTime;

        // Vector final
        velocity.y = verticalVelocity;

        // Mover y obtener flags reales de colisión
        CollisionFlags flags = characterController.Move(velocity * Time.deltaTime);

        // Grounded real del movimiento de este frame
        isGrounded = (flags & CollisionFlags.Below) != 0;

        // Si acabamos grounded, corta la caída acumulada inmediatamente
        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
    }



/*
    private void ControlMovimiento()
    {
        isGrounded = characterController.isGrounded;

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
            animator.SetTrigger("Saltar");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;

        }


        /////////Gravedad
        verticalVelocity += gravity * Time.deltaTime;
          Vector3 velocity = horizontalVelocity;
         velocity.y = verticalVelocity;
        horizontalVelocity.y = verticalVelocity;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }
    */
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f)
        {
            Debug.Log("Suelo detectado");
       //     isGrounded = true;
        }
    }
    
    
}