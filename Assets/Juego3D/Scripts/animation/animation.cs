using System;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class animation : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    [Tooltip("velocidad maxima utilizada para normalizar el movimiento")]
    private float velocidadMax = 1f;

    private Vector3 movimientoLocal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarMovimiento();


    }

    private void ActualizarMovimiento()
    {
        Vector3 velocidad = characterController.velocity;
        movimientoLocal = transform.InverseTransformDirection(velocidad);
        float x = movimientoLocal.x;
        float y = movimientoLocal.z;
        float Z = movimientoLocal.y;

        if (velocidadMax > 0)
        {
            x /= velocidadMax;
            y /= velocidadMax;
        }

        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
        animator.SetBool("suelo", characterController.isGrounded);
        animator.SetBool("dance", PlayerMovement.dance);
        animator.SetFloat("Z", Z);

    }

}
