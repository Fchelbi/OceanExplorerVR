using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private InputAction interactAction;

    // hashed IDs (course 5.2)
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int PickUpHash = Animator.StringToHash("PickUp");

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        animator.SetFloat(SpeedHash, movement.CurrentSpeed);
        animator.SetBool(IsGroundedHash, movement.IsGrounded);

        if (interactAction.WasPressedThisFrame())
            animator.SetTrigger(PickUpHash);
    }
}