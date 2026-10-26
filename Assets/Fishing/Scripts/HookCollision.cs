using UnityEngine;
using TMPro; // Pour TextMeshPro

public class HookCollision : MonoBehaviour
{
    private static int score = 0;
    private TextMeshProUGUI scoreUI;
    private bool hasCaughtSomething = false;

   
    public static void ResetScore()
    {
        score = 0;
    }

    void Start()
    {
        GameObject textObj = GameObject.Find("ScoreText");
        if (textObj != null)
        {
            scoreUI = textObj.GetComponent<TextMeshProUGUI>();
            UpdateUI();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasCaughtSomething) return;

        Debug.Log("L'hameçon a touché : " + other.gameObject.name + " | Tag : " + other.tag);

        if (other.CompareTag("Fish"))
        {
            hasCaughtSomething = true;
            Debug.Log("🎉 POISSON ATTRAPÉ !");

            if (other.transform.parent != null)
                Destroy(other.transform.parent.gameObject);
            else
                Destroy(other.gameObject);

            score += 50;
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