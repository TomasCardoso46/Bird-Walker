using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;      // Prefab to spawn
    [SerializeField] private Transform spawnPoint;          // Optional: custom spawn location
    private Transform parentObject;        // Parent under which the new prefab will be placed

    private void Start()
    {
        parentObject = GameObject.FindGameObjectWithTag("Mover").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawner"))
        {
            Vector3 position = spawnPoint ? spawnPoint.position : transform.position;
            GameObject newObject = Instantiate(prefabToSpawn, position, Quaternion.identity);

            if (parentObject != null)
            {
                newObject.transform.SetParent(parentObject);
            }
        }
    }
}
