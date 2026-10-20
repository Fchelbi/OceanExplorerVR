using UnityEngine;

public class FlottaisonVictime : MonoBehaviour
{
    public float vitesseFlottaison = 2f;
    public float hauteurFlottaison = 0.2f;
    private Vector3 positionInitiale;
    private Rigidbody rb;

    void Start()
    {
        positionInitiale = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calcul de la vague
        float nouvellePositionY = positionInitiale.y + Mathf.Sin(Time.time * vitesseFlottaison) * hauteurFlottaison;
        
        // On garde la position X et Z actuelles (pour qu'elle puisse être poussée), 
        // mais on force le Y pour la faire flotter.
        Vector3 nouvellePosition = new Vector3(rb.position.x, nouvellePositionY, rb.position.z);
        
        // On déplace via la physique
        rb.MovePosition(nouvellePosition);
    }
}