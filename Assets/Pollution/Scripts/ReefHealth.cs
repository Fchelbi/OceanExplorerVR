using UnityEngine;

public class ReefHealth : MonoBehaviour
{
    [SerializeField] private Renderer[] corals;
    [SerializeField] private Color deadColor = Color.gray;
    [SerializeField] private Color[] healthyColors;
    [SerializeField] private float checkInterval = 0.5f;

    private Vector3[] baseScales;
    private float[] baseHeights;
    private int initialTrash = 0;
    private int lastRemaining = -1;
    private float nextCheck;

    void Start()
    {
        baseScales = new Vector3[corals.Length];
        baseHeights = new float[corals.Length];

        for (int i = 0; i < corals.Length; i++)
        {
            baseScales[i] = corals[i].transform.localScale;
            baseHeights[i] = corals[i].transform.position.y;
        }
    }

    void Update()
    {
        if (Time.time < nextCheck) return;
        nextCheck = Time.time + checkInterval;

        int remaining = CountTrash();

        if (initialTrash == 0) initialTrash = remaining;
        if (initialTrash == 0 || remaining == lastRemaining) return;
        lastRemaining = remaining;

        float health = 1f - (float)remaining / initialTrash;
        UpdateCorals(health);

        int pollution = Mathf.RoundToInt(100f * remaining / initialTrash);
        Debug.Log("Pollution : " + pollution + " %");

        if (remaining == 0)
        {
            Debug.Log("Le récif est sauvé ! Tous les déchets ont été triés.");
        }
    }

    int CountTrash()
    {
        return GameObject.FindGameObjectsWithTag("Plastic").Length
             + GameObject.FindGameObjectsWithTag("Glass").Length
             + GameObject.FindGameObjectsWithTag("Metal").Length;
    }

    void UpdateCorals(float health)
    {
        float sizeFactor = Mathf.Lerp(0.7f, 1.2f, health);

        for (int i = 0; i < corals.Length; i++)
        {
            Color target = healthyColors[i % healthyColors.Length];
            corals[i].material.color = Color.Lerp(deadColor, target, health);

            corals[i].transform.localScale = baseScales[i] * sizeFactor;

            Vector3 pos = corals[i].transform.position;
            pos.y = baseHeights[i] * sizeFactor;
            corals[i].transform.position = pos;
        }
    }
}