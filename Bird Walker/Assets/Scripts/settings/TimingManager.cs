using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public static TimingManager Instance;

    private const string TimingKey = "TimingValue";
    private float timingValue = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadTiming();
    }

    public void SetTiming(float value)
    {
        timingValue = value;

        PlayerPrefs.SetFloat(TimingKey, timingValue);
        PlayerPrefs.Save();
    }

    public float GetTiming()
    {
        return timingValue;
    }

    private void LoadTiming()
    {
        timingValue = PlayerPrefs.GetFloat(TimingKey, 1f);
    }
}
