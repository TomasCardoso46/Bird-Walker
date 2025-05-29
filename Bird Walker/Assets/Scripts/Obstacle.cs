using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private MoveOnXAxis moveScript;

    void Start()
    {
        moveScript = FindObjectOfType<MoveOnXAxis>();

        if (moveScript == null)
        {
            Debug.LogWarning("MoveOnXAxis script not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle");

            if (moveScript != null)
            {
                moveScript.DisableScript();
            }
        }
    }
}
