using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOnXAxis : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Timing")]
    public float releaseDurationLimit = 2f; // will be overwritten by timing system

    [Header("Optional GameObject Toggles")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;

    [Header("Ragdolls")]
    [SerializeField] private ForceApplier upperRight, lowerRight, upperLeft, lowerLeft;
    [SerializeField] private float force;

    [Header("Saving")]
    [SerializeField] private MovementDistanceTracker movementDistanceTracker;

    private float pReleaseTimer = 0f;
    private float qReleaseTimer = 0f;

    private KeyCode? lastKeyPressed = null;
    private bool gameStarted = false;
    private bool gameActive = false;

    void Update()
    {
        bool pHeld = Input.GetKey(KeyCode.P);
        bool qHeld = Input.GetKey(KeyCode.Q);

        // ---------- READY SYSTEM ----------
        if (!gameStarted)
        {
            if (pHeld && qHeld)
            {
                Debug.Log("Both keys held - READY!");
                // Try to find ApplyTimingOnStart
                ApplyTimingOnStart timingScript = FindObjectOfType<ApplyTimingOnStart>();

                if (timingScript != null)
                {
                    // Use timing value applied in this scene
                    releaseDurationLimit = timingScript.gameplayTiming;
                    Debug.Log("Timing applied to MoveOnXAxis: " + releaseDurationLimit);
                }
                else
                {
                    // No timing script → use default
                    releaseDurationLimit = 0.5f;
                    Debug.Log("No ApplyTimingOnStart found. Using default timing: 0.5");
                }
                gameStarted = true;
                gameActive = true;
            }
            return;
        }

        // ---------- GAMEPLAY ----------
        if (!gameActive) return;

        // --- Double press check ---
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (lastKeyPressed == KeyCode.Q)
            {
                Debug.Log("Q pressed twice. Failure!");
                upperLeft.ApplyForce(0, force, 0);
                DisableScript();
                return;
            }
            lastKeyPressed = KeyCode.Q;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (lastKeyPressed == KeyCode.P)
            {
                Debug.Log("P pressed twice. Failure!");
                upperRight.ApplyForce(0, force, 0);
                DisableScript();
                return;
            }
            lastKeyPressed = KeyCode.P;
        }

        // --- FAILURE: both not held ---
        if (!pHeld && !qHeld)
        {
            Debug.Log("Both keys released - FAILURE!");
            upperLeft.ApplyForce(0, 0, 0);
            DisableScript();
            return;
        }

        // --- P Release Timer ---
        if (!pHeld)
        {
            pReleaseTimer += Time.deltaTime;
            if (pReleaseTimer >= releaseDurationLimit)
            {
                Debug.Log("P not held too long - FAILURE!");
                lowerRight.ApplyForce(0, 0, force);
                DisableScript();
                return;
            }
        }
        else pReleaseTimer = 0f;

        // --- Q Release Timer ---
        if (!qHeld)
        {
            qReleaseTimer += Time.deltaTime;
            if (qReleaseTimer >= releaseDurationLimit)
            {
                Debug.Log("Q not held too long - FAILURE!");
                lowerLeft.ApplyForce(0, 0, force);
                DisableScript();
                return;
            }
        }
        else qReleaseTimer = 0f;

        // --- MOVEMENT ---
        if ((pHeld && !qHeld) || (!pHeld && qHeld))
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void DisableScript()
    {
        gameActive = false;

        if (movementDistanceTracker != null)
            movementDistanceTracker.StopAndSave();

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        //if (objectToDeactivate != null)
        //    objectToDeactivate.SetActive(false);

        StartCoroutine(LoadMenuAfterDelay());
    }

    private IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
        enabled = false;
    }
}
