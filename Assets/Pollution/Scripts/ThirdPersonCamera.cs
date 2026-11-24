using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;          // Player
    [SerializeField] private float distance = 4f;
    [SerializeField] private float height = 1.5f;
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.3f;
    [SerializeField] private float smoothTime = 0.05f;

    private float yaw;
    private float pitch = 20f;
    private Vector3 currentVelocity;
    private InputAction lookAction;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = target.eulerAngles.y;
    }

    void LateUpdate()
    {
        // 1. Mouse turns the camera
        Vector2 look = lookAction.ReadValue<Vector2>();
        yaw += look.x * sensitivity;
        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 2. Wanted position behind the player
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 focus = target.position + Vector3.up * height;
        Vector3 direction = rotation * Vector3.back;
        float finalDistance = distance;

        // 3. If something is between player and camera, come closer
        if (Physics.SphereCast(focus, collisionRadius, direction, out RaycastHit hit, distance, collisionMask))
            finalDistance = hit.distance;

        Vector3 desiredPosition = focus + direction * finalDistance;

        // 4. Move smoothly and look at the player
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.LookAt(focus);
    }
}