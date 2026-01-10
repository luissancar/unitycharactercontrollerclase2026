using System;
using UnityEngine;
public class AnimacionesPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;


    void LateUpdate()
    {
        Vector3 velocidad = characterController.velocity;
        Vector3 movimientoLocal = characterController.transform.InverseTransformDirection(velocidad);

        animator.SetFloat("X", movimientoLocal.x);
        animator.SetFloat("Y", movimientoLocal.z);
        animator.SetBool("EnSuelo", characterController.isGrounded);
    }
}
