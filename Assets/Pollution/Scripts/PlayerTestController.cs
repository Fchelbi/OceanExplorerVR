using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTestController : MonoBehaviour
{
    [SerializeField] private float force = 20f;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(direction * force, ForceMode.Force);
    }
}