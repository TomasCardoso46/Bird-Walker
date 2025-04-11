using UnityEngine;

public class MoveOnXAxis : MonoBehaviour
{
    public float speed = 5f;

    public float pHoldDurationLimit = 2f; // seconds
    public float qHoldDurationLimit = 2f; // seconds

    private float pHoldTimer = 0f;
    private float qHoldTimer = 0f;

    void Update()
    {
        bool qHeld = Input.GetKey(KeyCode.Q);
        bool pHeld = Input.GetKey(KeyCode.P);

        // Disable if both keys are held
        if (qHeld && pHeld)
        {
            Debug.Log("Both Q and P held. Disabling script.");
            DisableScript();
            return;
        }

        // Handle P hold time
        if (pHeld)
        {
            pHoldTimer += Time.deltaTime;
            if (pHoldTimer >= pHoldDurationLimit)
            {
                Debug.Log("P held too long. Disabling script.");
                DisableScript();
                return;
            }
        }
        else
        {
            pHoldTimer = 0f;
        }

        // Handle Q hold time
        if (qHeld)
        {
            qHoldTimer += Time.deltaTime;
            if (qHoldTimer >= qHoldDurationLimit)
            {
                Debug.Log("Q held too long. Disabling script.");
                DisableScript();
                return;
            }
        }
        else
        {
            qHoldTimer = 0f;
        }

        // Move if only one key is held
        if (qHeld || pHeld)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    void DisableScript()
    {
        enabled = false;
    }
}
