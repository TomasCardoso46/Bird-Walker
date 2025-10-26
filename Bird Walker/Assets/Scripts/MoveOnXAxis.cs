using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOnXAxis : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Timing")]
    public float releaseDurationLimit = 2f; // how long a key can stay released before failure

    [Header("Optional GameObject Toggles")]
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;


    [Header("Ragdolls")]
    [SerializeField] private ForceApplier upperRight, lowerRight, upperLeft, lowerLeft;
    [SerializeField] private float force;

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
                gameStarted = true;
                gameActive = true;
            }
            return; // wait until both are held once
        }

        // ---------- GAMEPLAY ----------
        if (!gameActive) return;

        // --- Double press check (fail-safe) ---
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

        // --- FAILURE CONDITION: both not held ---
        if (!pHeld && !qHeld)
        {
            Debug.Log("Both keys released - FAILURE!");
            upperLeft.ApplyForce(0, 0, 0);
            DisableScript();
            return;
        }

        // --- Handle release timers ---
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

        // --- MOVEMENT LOGIC ---
        // Move when exactly one key is released (one held, one not)
        if ((pHeld && !qHeld) || (!pHeld && qHeld))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        // If both held — idle (no movement)
    }

    public void DisableScript()
    {
        gameActive = false;

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        //if (objectToDeactivate != null)
            //objectToDeactivate.SetActive(false);

        StartCoroutine(LoadMenuAfterDelay());
    }

    private IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
        enabled = false;
    }
}
