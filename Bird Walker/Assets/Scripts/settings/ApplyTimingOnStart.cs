using UnityEngine;

public class ApplyTimingOnStart : MonoBehaviour
{
    
    public float gameplayTiming = 1f;

    private void Start()
    {
        if (TimingManager.Instance != null)
        {
            gameplayTiming = TimingManager.Instance.GetTiming();
            // Apply to whatever script/behavior needs this timing
        }

        Debug.Log("Timing applied: " + gameplayTiming);
    }
}
