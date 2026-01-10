using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 15f;
    
    private CharacterController cc;
    private Vector3 move;
    private float ySpeed;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (cc == null) cc = gameObject.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.center = new Vector3(0, 1, 0);
    }
    
    void Update()
    {
        // Movimiento básico
        cc.Move(move * speed * Time.deltaTime);
        
        // Gravedad
        ySpeed += Physics.gravity.y * Time.deltaTime;
        
        // Mover en Y
        cc.Move(new Vector3(0, ySpeed * Time.deltaTime, 0));
        
        // DEBUG: Chequear suelo
        Debug.Log("Grounded: " + cc.isGrounded + " | Y Speed: " + ySpeed);
    }
    
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        move = new Vector3(input.x, 0, input.y);
        move = transform.TransformDirection(move);
    }
    
    public void OnJump(InputValue value)
    {
        Debug.Log("JUMP BUTTON PRESSED!");
        
        if (value.isPressed && cc.isGrounded)
        {
            ySpeed = jumpForce;
            Debug.Log("APPLYING JUMP FORCE!");
        }
    }
}
