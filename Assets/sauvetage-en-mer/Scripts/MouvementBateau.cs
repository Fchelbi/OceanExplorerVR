using UnityEngine;

public class MouvementBateau : MonoBehaviour
{
    public float vitesse = 10f;
    public float vitesseRotation = 50f;
    private Rigidbody rb;

    void Start()
    {
        // On récupère le composant physique du bateau
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate est utilisé pour la physique (au lieu de Update)
    void FixedUpdate() 
    {
        float avancer = Input.GetAxis("Vertical");
        float tourner = Input.GetAxis("Horizontal");

        // Transformation physique : Rotation
        Quaternion rotation = Quaternion.Euler(0, tourner * vitesseRotation * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rotation);

        // Transformation physique : Translation
        Vector3 mouvement = transform.forward * avancer * vitesse * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + mouvement);
    }
}