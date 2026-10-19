using UnityEngine;
using TMPro; // <--- On utilise TextMeshPro

public class HookCollision : MonoBehaviour
{
    private static int score = 0;
    private TextMeshProUGUI scoreUI; // <--- Le type a changé

    void Start()
    {
        GameObject textObj = GameObject.Find("ScoreText");
        if (textObj != null)
        {
            // On cherche le composant TextMeshProUGUI
            scoreUI = textObj.GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }
        else
        {
            Debug.LogError("Attention : Le GameObject 'ScoreText' est introuvable !");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
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