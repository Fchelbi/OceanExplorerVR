using UnityEngine;

public class RegardSouris : MonoBehaviour
{
    public float sensibilité = 2f;
    private float rotationX = 0;
    private float rotationY = 0;

    void Start()
    {
        // On cache et on bloque le curseur au centre de l'écran
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Récupère le mouvement de la souris
        float mouseX = Input.GetAxis("Mouse X") * sensibilité;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilité;

        // Calcule la rotation (haut/bas et gauche/droite)
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Empêche de se retourner complètement

        // Applique la rotation à la caméra (Transformation géométrique)
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}