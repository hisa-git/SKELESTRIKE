using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 1f;
    public float sprintMultiplier = 1.5f;
    public float acceleration = 1f;
    public float deceleration = 1f;
    public float jumpStrenght = 1f;
    public float fallAcceleration = 1f;
    private float speedMultiplier = 1f;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;
    private Vector2 moveInput;

    public Transform camTransform;
    public PlatformTrigger platTrigger;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        moveAction.action.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();
        jumpAction.action.performed += Jump;
        sprintAction.action.performed += SprintStart;
        sprintAction.action.canceled += SprintStop;
    }
    void OnDisable()
    {
        moveAction.action.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled -= ctx => moveInput = ctx.ReadValue<Vector2>();
        jumpAction.action.performed -= Jump;
        moveAction.action.Disable();
        sprintAction.action.Disable();
        jumpAction.action.Disable();
    }
    void Update()
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;
        moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * maxSpeed * speedMultiplier;

        float currentAcceleration = (moveInput.magnitude > 0.1f) ? acceleration : deceleration;

        Vector3 moveForce = currentAcceleration * new Vector3(targetVelocity.x - rb.linearVelocity.x, 0, targetVelocity.z - rb.linearVelocity.z);

        rb.AddForce(moveForce, ForceMode.Acceleration);

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(new Vector3(0, -fallAcceleration, 0), ForceMode.Acceleration);
        }
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (platTrigger.isOnPlatform)
        {
            Vector3 jumpForce = new Vector3(0, jumpStrenght, 0);
            rb.AddForce(jumpForce, ForceMode.Impulse);
            CoolnessManager.Add(5);
        }
    }

    void SprintStart(InputAction.CallbackContext context)
    {
        speedMultiplier = sprintMultiplier;
    }
    void SprintStop(InputAction.CallbackContext context)
    {
        speedMultiplier = 1f;
    }
}
