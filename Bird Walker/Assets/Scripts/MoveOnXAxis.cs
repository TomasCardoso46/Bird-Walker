using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoveOnXAxis : MonoBehaviour
{
    public float speed = 5f;

    public float pHoldDurationLimit = 2f; // seconds
    public float qHoldDurationLimit = 2f; // seconds

    private float pHoldTimer = 0f;
    private float qHoldTimer = 0f;

    private KeyCode? lastKeyPressed = null;

    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;

    void Update()
    {
        bool qHeld = Input.GetKey(KeyCode.Q);
        bool pHeld = Input.GetKey(KeyCode.P);

        // Detect key down to check for double press
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (lastKeyPressed == KeyCode.Q)
            {
                Debug.Log("Q pressed twice. Disabling script.");
                DisableScript();
                return;
            }
            lastKeyPressed = KeyCode.Q;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (lastKeyPressed == KeyCode.P)
            {
                Debug.Log("P pressed twice. Disabling script.");
                DisableScript();
                return;
            }
            lastKeyPressed = KeyCode.P;
        }

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

    public void DisableScript()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        StartCoroutine(LoadMenuAfterDelay());
        
    }
    private IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
        enabled = false;
    }
}
