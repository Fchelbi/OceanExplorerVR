using UnityEngine;

public class LancerBouee : MonoBehaviour
{
    [Header("Paramètres de lancement")]
    public GameObject prefabBouee; // La case pour glisser le Prefab
    public Transform pointLancement; // La case pour glisser le PointLancement
    public float forceLancement = 15f;

    void Update()
    {
        // Gestion des entrées utilisateur : Espace pour lancer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Lancer();
        }
    }

    void Lancer()
    {
        // 1. INSTANCIATION : On crée la bouée à la position du point de lancement
        GameObject nouvelleBouee = Instantiate(prefabBouee, pointLancement.position, pointLancement.rotation);

        // 2. PHYSIQUE : On récupère le Rigidbody de la bouée instanciée pour lui ajouter une force
        Rigidbody rbBouee = nouvelleBouee.GetComponent<Rigidbody>();
        
        if (rbBouee != null)
        {
            // On la propulse vers l'avant
            rbBouee.AddForce(pointLancement.forward * forceLancement, ForceMode.Impulse);
        }

        // 3. DESTRUCTION (Bonus) : On détruit la bouée après 10 secondes pour nettoyer la scène
        //Destroy(nouvelleBouee, 10f); 
    }
}