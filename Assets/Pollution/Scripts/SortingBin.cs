using UnityEngine;

public class SortingBin : MonoBehaviour
{
    [SerializeField] private string acceptedTag;

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (obj.CompareTag(acceptedTag))
        {
            Destroy(obj);
            Debug.Log("Bon tri ! Déchet " + acceptedTag + " recyclé.");
        }
        else if (obj.CompareTag("Plastic") || obj.CompareTag("Glass") || obj.CompareTag("Metal"))
        {
            Debug.Log("Mauvaise poubelle ! Ce déchet est : " + obj.tag);
        }
    }
}