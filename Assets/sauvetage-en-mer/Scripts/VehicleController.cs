using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float acceleration = 12f;
    public float turnSpeed = 60f;
    public float maxSpeed = 8f;

    Rigidbody rb;
    float moveInput;
    float turnInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");   // flèches haut/bas
        turnInput = Input.GetAxis("Horizontal"); // flèches gauche/droite
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f));
    }
}