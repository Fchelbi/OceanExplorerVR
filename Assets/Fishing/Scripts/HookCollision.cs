using UnityEngine;
using TMPro;

public class HookCollision : MonoBehaviour
{
    private static int score = 0;
    private TextMeshProUGUI scoreUI;
    private bool hasCaughtSomething = false; // <-- Sécurité anti-double collision

    void Start()
    {
        // On remet le score à 0 au démarrage du jeu
        if (score > 0 && Time.time < 1f) score = 0;

        GameObject textObj = GameObject.Find("ScoreText");
        if (textObj != null)
        {
            scoreUI = textObj.GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si on a déjà attrapé quelque chose avec CET hameçon, on ignore
        if (hasCaughtSomething) return;

        if (other.CompareTag("Fish"))
        {
            hasCaughtSomething = true; // On bloque les autres collisions
            Debug.Log("🎉 POISSON ATTRAPÉ !");

            if (other.transform.parent != null)
                Destroy(other.transform.parent.gameObject);
            else
                Destroy(other.gameObject);

            score += 10;
            UpdateUI();

            Destroy(gameObject);
        }

        if (other.CompareTag("Water"))
        {
            Destroy(gameObject, 2f);
        }
    }

    void UpdateUI()
    {
        if (scoreUI != null)
        {
            scoreUI.text = "Score: " + score;
        }
    }
}