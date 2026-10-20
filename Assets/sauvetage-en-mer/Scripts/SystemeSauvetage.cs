using UnityEngine;
using UnityEngine.UI; // Nécessaire pour l'UI

public class SystemeSauvetage : MonoBehaviour
{
    [Header("Interface Utilisateur")]
    public GameObject texteUI; // Le texte qui dira "Appuyez sur E pour sauver"
    
    [Header("Paramètres de Sauvetage")]
    public float distanceMaxSauvetage = 5f; // Distance maximale pour récupérer la victime
    private bool victimeDansLaZone = false;

    void Start()
    {
        // On cache le texte au début du jeu
        if (texteUI != null) texteUI.SetActive(false);
    }

    // Détection de collision (Entrée dans la zone)
    void OnTriggerEnter(Collider other)
    {
        // Recherche et manipulation : On vérifie si l'objet a le tag "Victime"
        if (other.CompareTag("Victime"))
        {
            victimeDansLaZone = true;
            Debug.Log("Victime repérée ! Prêt à sauver.");
            if (texteUI != null) texteUI.SetActive(true); // On affiche l'UI
        }
    }

    // Détection de collision (Sortie de la zone)
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Victime"))
        {
            victimeDansLaZone = false;
            if (texteUI != null) texteUI.SetActive(false); // On cache l'UI
        }
    }

    void Update()
    {
        // Gestion des entrées utilisateur : Appuyer sur E pour sauver
        if (victimeDansLaZone && Input.GetKeyDown(KeyCode.E))
        {
            SauverLaVictime();
        }
    }

    void SauverLaVictime()
    {
        // Recherche de GameObject : On cherche l'objet avec le tag "Victime" dans la scène
        GameObject victime = GameObject.FindWithTag("Victime");
        
        if (victime != null)
        {
            Debug.Log("Sauvetage réussi ! La victime est récupérée.");
            
            // Destruction d'objet : On détruit la capsule de la victime
            Destroy(victime);
            
            // On cache l'UI
            if (texteUI != null) texteUI.SetActive(false);
            victimeDansLaZone = false;
        }
    }
}