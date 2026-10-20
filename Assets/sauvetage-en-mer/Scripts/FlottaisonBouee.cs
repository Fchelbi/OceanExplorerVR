using UnityEngine;

public class FlottaisonBouee : MonoBehaviour
{
    public float hauteurFlottaison = 0.5f; // La hauteur à laquelle elle doit flotter
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Si la bouée est en dessous de la surface, on la pousse vers le haut (Poussée d'Archimède)
        if (rb.position.y < hauteurFlottaison)
        {
            // On ajoute une force vers le haut
            rb.AddForce(Vector3.up * 10f, ForceMode.Acceleration);
        }
    }
}