using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// First-person movement for Bulwark: walking, jumping, gravity, and mouse look.
/// Uses a CharacterController and the new Input System.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child object that holds the camera. Pitch (up/down look) rotates this.")]
    [SerializeField] private Transform cameraPivot;

    [Header("Input (from InputSystem_Actions > Player)")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.5f;      // Tanks feel a little heavier
    [SerializeField] private float acceleration = 40f;    // How fast you reach full speed
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float maxPitch = 85f;

    private CharacterController controller;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private float pitch;

    // Other systems (like the tether) can read this later.
    public bool IsGrounded => controller.isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        // Mouse delta is already per-frame, so no Time.deltaTime here.
        Vector2 look = lookAction.action.ReadValue<Vector2>() * lookSensitivity;

        // Yaw turns the whole body; pitch only tilts the camera.
        transform.Rotate(Vector3.up * look.x);

        pitch = Mathf.Clamp(pitch - look.y, -maxPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 targetVelocity = (transform.right * input.x + transform.forward * input.y) * moveSpeed;

        // Smoothly accelerate toward the target speed instead of snapping.
        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);

        if (controller.isGrounded)
        {
            // Small downward push keeps the controller stuck to slopes.
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (jumpAction.action.WasPressedThisFrame())
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
