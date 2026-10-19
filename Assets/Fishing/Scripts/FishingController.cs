using UnityEngine;

public class FishingController : MonoBehaviour
{
    public GameObject hookPrefab;    // La sphère (Hameçon)
    public Transform rodTip;         // Le bout de la canne (BoutCanne)
    public float castForce = 15f;    // La force du lancer

    void Update()
    {
        // Détecte le clic gauche de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic détecté ! Je lance l'hameçon.");
            CastLine();
        }
    }

    void CastLine()
    {
        // Vérifie qu'on a bien assigné les objets
        if (hookPrefab == null || rodTip == null)
        {
            Debug.LogError("ERREUR : Il manque le Prefab de l'hameçon ou le BoutCanne dans l'Inspector !");
            return;
        }

        // 1. Instanciation de l'hameçon au bout de la canne
        GameObject newHook = Instantiate(hookPrefab, rodTip.position, rodTip.rotation);

        // 2. Ajout de la physique (propulsion)
        Rigidbody rb = newHook.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // On le propulse vers l'avant de la caméra
            rb.AddForce(Camera.main.transform.forward * castForce, ForceMode.Impulse);
        }
    }
}