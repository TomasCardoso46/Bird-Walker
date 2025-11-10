using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI highestScoreText;

    void Start()
    {
        float last = PlayerPrefs.GetFloat("LastScore", 0f);
        float high = PlayerPrefs.GetFloat("HighestScore", 0f);

        lastScoreText.text = "Last Score: " + last.ToString("F1") + " m";
        highestScoreText.text = "Highest Score: " + high.ToString("F1") + " m";
    }
}
