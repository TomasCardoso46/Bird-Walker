using UnityEngine;
using TMPro;

public class MovementDistanceTracker : MonoBehaviour
{
    public MoveOnXAxis moveScript;
    public TextMeshProUGUI scoreText;

    private Vector3 lastPos;
    private float movementTime = 0f;
    private bool stopped = false;

    void Start()
    {
        lastPos = moveScript.transform.position;
    }

    void Update()
    {
        if (stopped)
            return;

        // If movement script disabled -> failure detected
        if (!moveScript.enabled)
        {
            StopAndSave();
            return;
        }

        // movement detection
        Vector3 currentPos = moveScript.transform.position;
        float dist = Vector3.Distance(currentPos, lastPos);

        if (dist > 0.0001f) // movement happened this frame
        {
            movementTime += Time.deltaTime;
        }

        lastPos = currentPos;

        float meters = Mathf.Round(movementTime * 10f) / 10f;

        scoreText.text = meters.ToString("F1");
    }

    public void StopAndSave()
    {
        stopped = true;

        float meters = Mathf.Round(movementTime * 10f) / 10f;

        PlayerPrefs.SetFloat("LastScore", meters);

        float highest = PlayerPrefs.GetFloat("HighestScore", 0f);
        if (meters > highest)
            PlayerPrefs.SetFloat("HighestScore", meters);

        PlayerPrefs.Save();
    }
}
