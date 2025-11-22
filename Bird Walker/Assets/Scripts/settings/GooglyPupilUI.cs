using UnityEngine;
using UnityEngine.UI;

public class GooglyPupilSlider : MonoBehaviour
{
    public Slider slider;         // Assign your slider
    public RectTransform pupil;   // The pupil inside the eye
    public RectTransform eye;     // The eye containing the pupil

    public float maxOffset = 8f;      // How far pupil can move
    public float followSpeed = 10f;   // How fast it follows

    void Update()
    {
        // Map slider value (0-1) to local eye offset
        float normalized = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);

        // For horizontal slider: move pupil left/right
        Vector2 targetPos = new Vector2(
            Mathf.Lerp(-maxOffset, maxOffset, normalized),
            0f
        );

        // Smoothly move pupil
        pupil.anchoredPosition = Vector2.Lerp(
            pupil.anchoredPosition,
            targetPos,
            followSpeed * Time.deltaTime
        );
    }
}
