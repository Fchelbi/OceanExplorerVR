using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f; // Haut / Bas
    private float yRotation = 0f; // Gauche / Droite

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Si on clique, on s'assure que la souris est bien capturée dans le jeu
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calcul de la rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f); // Limite pour ne pas se casser le cou

        yRotation += mouseX;

        // On applique les deux rotations en même temps sur l'axe local de la caméra
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}