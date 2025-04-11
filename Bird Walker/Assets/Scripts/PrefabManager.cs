using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    public GameObject prefabToSpawn;      // Prefab to spawn
    public Transform spawnPoint;          // Optional: custom spawn location
    public Transform parentObject;        // Parent under which the new prefab will be placed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
