using UnityEngine;
using UnityEngine.UI;

public class Fisch : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timerDuration = 3f;
    private float currentTime = 0f;
    private bool isInZone = false;
    private bool hasFisched = false;

    [Header("X-Axis Bounds (World Position)")]
    public RectTransform minX;
    public RectTransform maxX;

    [Header("Movement Settings")]
    public float moveSpeed = 50f; // pixels per second

    private RectTransform rectTransform;
    private float targetX;

    [Header("Random Movement Settings")]
    public float changeTargetInterval = 2f; // time between direction changes
    private float targetChangeTimer = 0f;

    public GameObject anzol;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        PickNewTargetX();
    }

    private void Update()
    {
        // Handle movement
        MoveRandomlyX();

        // Update timer for changing direction
        targetChangeTimer += Time.deltaTime;
        if (targetChangeTimer >= changeTargetInterval)
        {
            PickNewTargetX();
            targetChangeTimer = 0f;
        }

        // Handle fishing logic
        if (isInZone && !hasFisched)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timerDuration)
            {
                Fisched();
                hasFisched = true;
            }
        }
    }

    private void MoveRandomlyX()
    {
        float step = moveSpeed * Time.deltaTime;
        Vector3 currentPosition = rectTransform.anchoredPosition;
        currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetX, step);
        rectTransform.anchoredPosition = currentPosition;
    }

    private void PickNewTargetX()
    {
        float min = minX.anchoredPosition.x;
        float max = maxX.anchoredPosition.x;
        targetX = Random.Range(min, max);
    }

    private void Fisched()
    {
        Debug.Log("Fisched");
        // Add logic for catching the fish here
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == anzol)
        {
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == anzol)
        {
            isInZone = false;
        }
    }
}
