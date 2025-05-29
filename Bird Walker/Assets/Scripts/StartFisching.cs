using UnityEngine;

public class StartFisching : MonoBehaviour
{
    private bool hasFisched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFisched && other.CompareTag("Player"))
        {
            // Enable all direct children of all objects tagged "Fisch"
            GameObject[] fischObjects = GameObject.FindGameObjectsWithTag("Fisch");

            if (fischObjects.Length == 0)
            {
                Debug.LogWarning("No objects found with tag 'Fisch'.");
            }
            else
            {
                foreach (GameObject fischParent in fischObjects)
                {
                    foreach (Transform child in fischParent.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }

            // Disable all MoveOnXAxis scripts in the scene
            DisableScriptsByName("MoveOnXAxis");

            // Disable all CheckpointMover scripts in the scene
            //DisableScriptsByName("CheckpointMover");

            hasFisched = true;
            Debug.Log("Fisching started: children enabled and all specified scripts disabled.");
        }
    }

    private void DisableScriptsByName(string scriptName)
    {
        // Find all MonoBehaviours in the scene
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>(true);

        foreach (var script in allScripts)
        {
            if (script.GetType().Name == scriptName)
            {
                script.enabled = false;
                Debug.Log($"Disabled {scriptName} script on {script.gameObject.name}");
            }
        }
    }
}
