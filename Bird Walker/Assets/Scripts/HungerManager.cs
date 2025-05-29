using UnityEngine;

public class HungerManager : MonoBehaviour
{
    public float hunger = 100f;           // Current hunger
    public float maxHunger = 100f;        // Maximum hunger value
    public float depletionSpeed = 5f;     // Hunger depleted per second

    private MoveOnXAxis moveScript;

    private void Start()
    {
        hunger = maxHunger;
        moveScript = GetComponent<MoveOnXAxis>();
        if (moveScript == null)
        {
            Debug.LogWarning("MoveOnXAxis component not found on the GameObject.");
        }
    }

    private void Update()
    {
        if (hunger > 0)
        {
            hunger -= depletionSpeed * Time.deltaTime;
            if (hunger <= 0)
            {
                hunger = 0;
                if (moveScript != null)
                {
                    moveScript.DisableScript();
                }
            }
        }
    }

    public void GainHunger(float amount)
    {
        hunger += amount;
        if (hunger > maxHunger)
        {
            hunger = maxHunger;
        }
    }
}
