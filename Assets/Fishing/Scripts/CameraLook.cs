using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Start()
    {
        // Cache le curseur de la souris au centre de l'écran pour le jeu
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Récupère les mouvements de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calcul pour regarder en haut et en bas (limité pour ne pas se retourner)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // Applique la rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Tourne la caméra à gauche et à droite
        transform.Rotate(Vector3.up * mouseX);
    }
}