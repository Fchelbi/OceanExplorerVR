using UnityEngine;

public class RescueBuoy : MonoBehaviour
{
    public Victim victim;
    public float waterLevel = 0f;
    public float surfaceOffset = 0.05f;   // hauteur au-dessus de l'eau (augmente si elle paraît enfoncée)

    Rigidbody rb;
    bool landed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 0f;
    }

    void Update()
    {
        if (victim != null || landed) return;

        // Elle tombe, puis se pose sur la surface et ne bouge plus
        if (transform.position.y <= waterLevel + surfaceOffset && rb.linearVelocity.y < 0f)
        {
            landed = true;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            Vector3 p = transform.position;
            p.y = waterLevel + surfaceOffset;
            transform.position = p;
        }
    }

    void OnTriggerEnter(Collider other) { TryGrab(other); }
    void OnTriggerStay(Collider other) { TryGrab(other); }

    void TryGrab(Collider other)
    {
        if (victim != null) return;

        Victim v = other.GetComponentInParent<Victim>();
        if (v != null && v.state == VictimState.Drowning)
        {
            victim = v;
            if (!rb.isKinematic) rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            v.GrabBuoy(this);
        }
    }
}