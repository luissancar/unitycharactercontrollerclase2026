using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SlopeSlideOnly : MonoBehaviour
{
    [Header("Deslizamiento pendiente")]
    [SerializeField] private float slideSpeed = 4f;

    [SerializeField] private float minSlopeAngleToSlide = 3f;

    private CharacterController controller;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!controller.isGrounded)
            return;
        bool hasInput = moveInput.sqrMagnitude > 0.01f;
        if (hasInput)
            return;
        if (!IsOnSlope(out RaycastHit hitInfo))
            return;
        float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
        if (angle < minSlopeAngleToSlide)
            return;
        Vector3 slideDir =Vector3.ProjectOnPlane(Vector3.down, hitInfo.normal).normalized;

        Vector3 displacement = slideDir * slideSpeed * Time.deltaTime;
        controller.Move(displacement);
    }

    bool IsOnSlope(out RaycastHit hit)
    {
        Vector3 origin = controller.bounds.center;
        float rayLength = (controller.height / 2f) + 0.5f;
        if (Physics.Raycast(origin, Vector3.down, out hit, rayLength))
        {
            float angle =Vector3.Angle(hit.normal, Vector3.up);
            return angle > 0.01f;
        }

        return false;

    }
}
