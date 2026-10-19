using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 50f;

    void Update()
    {
        // Lire les touches
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // Affiche dans la console si on appuie sur une touche
        if (move != 0 || turn != 0)
        {
            Debug.Log("Mouvement détecté ! Move: " + move + " Turn: " + turn);
        }

        // Applique le mouvement
        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);
    }
}