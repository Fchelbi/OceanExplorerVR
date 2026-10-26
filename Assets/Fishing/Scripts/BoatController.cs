using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;

    [Header("Hauteur du bateau (Y)")]
    public float hauteurFixe = 0.66f; // <-- Change cette valeur pour la hauteur de TON bateau !

    [Header("Limites de la piscine")]
    public float minX = -25.8f;
    public float maxX = 36.2f;
    public float minZ = -34.05f;
    public float maxZ = 17.95f;

    void Update()
    {
        // Mouvement
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);

        // Limites de la piscine
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

        // LA LIGNE MAGIQUE : On force la position Y à rester bloquée sur 'hauteurFixe'
        transform.position = new Vector3(clampedX, hauteurFixe, clampedZ);
    }
}