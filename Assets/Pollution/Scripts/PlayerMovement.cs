using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    // read by PlayerAnimator (step 5)
    public float CurrentSpeed { get; private set; }
    public bool IsGrounded { get; private set; }
    public float VerticalSpeed => velocity.y;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Update()
    {
        // 1. Ground
        IsGrounded = controller.isGrounded;
        if (IsGrounded && velocity.y < 0) velocity.y = -2f;

        // 2. Camera-relative direction (course 2.4)
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0; camRight.y = 0;
        camForward.Normalize(); camRight.Normalize();
        Vector3 move = camForward * input.y + camRight * input.x;
        move = Vector3.ClampMagnitude(move, 1f);

        // 3. Walk or run
        float speed = sprintAction.IsPressed() ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);
        CurrentSpeed = move.magnitude * speed;

        // 4. Turn Sophie towards where she walks
        if (move != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, turnSpeed * Time.deltaTime);
        }

        // 5. Jump + gravity
        if (jumpAction.WasPressedThisFrame() && IsGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}