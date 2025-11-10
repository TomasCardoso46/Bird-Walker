using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 10f;
    public TextMeshProUGUI timerText;
    public MoveOnXAxis moveScript;

    private float currentTime;
    private bool timerRunning = true;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (!timerRunning) return;
        if (!moveScript.enabled) return; // script already failed

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            timerRunning = false;
            timerText.text = "0.0";

            TriggerFail();
            return;
        }

        timerText.text = currentTime.ToString("F1");
    }

    void TriggerFail()
    {
        // easiest way to simulate failure: disable script
        moveScript.enabled = false;
    }
}
