using UnityEngine;

public class FishingController : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform rodTip;
    public float castForce = 15f;

    void Start()
    {
        
        HookCollision.ResetScore();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clic détecté ! Je lance l'hameçon.");
            CastLine();
        }
    }

    void CastLine()
    {
        if (hookPrefab == null || rodTip == null) return;

        GameObject newHook = Instantiate(hookPrefab, rodTip.position, rodTip.rotation);
        Rigidbody rb = newHook.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Camera.main.transform.forward * castForce, ForceMode.Impulse);
        }
    }
}