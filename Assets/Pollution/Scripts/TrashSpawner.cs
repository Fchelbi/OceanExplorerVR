using UnityEngine;

public class TrashSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private int trashCount = 15;
    [SerializeField] private float spawnHeight = 6f;


    void Start()
    {
        for (int i = 0; i < trashCount; i++)
        {
            int index = Random.Range(0, trashPrefabs.Length);

            float x = Random.Range(-12f, 12f);
            float z = Random.Range(-9f, 12f);
            Vector3 position = new Vector3(x, spawnHeight, z);

            Instantiate(trashPrefabs[index], position, Quaternion.identity);
        }
        
    }

   
}
