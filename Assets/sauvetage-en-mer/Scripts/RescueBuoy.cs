using UnityEngine;

public class RescueBuoy : MonoBehaviour
{
    public Victim victim;
    public float lifeTime = 3f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Bouée ratée : elle disparaît après quelques secondes
        if (victim == null)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0f) Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (victim != null) return;

        Victim v = other.GetComponent<Victim>();
        if (v != null && v.state == VictimState.Drowning)
        {
            victim = v;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            v.GrabBuoy(this);
        }
    }
}