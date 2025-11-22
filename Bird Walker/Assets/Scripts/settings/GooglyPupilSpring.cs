using UnityEngine;
using UnityEngine.UI;

public class GooglyPupilSpring : MonoBehaviour
{
    public Slider slider;           // Assign your slider
    public RectTransform pupil;     // The pupil inside the eye
    public RectTransform eye;       // The eye containing the pupil

    public float maxOffset = 8f;    // Maximum distance pupil can move
    public float spring = 20f;      // Spring stiffness
    public float damping = 5f;      // Damping factor for velocity

    private Vector2 velocity;

    void Update()
    {
        // Map slider value (0-1) to target local position
        float normalized = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        Vector2 targetPos = new Vector2(
            Mathf.Lerp(-maxOffset, maxOffset, normalized),
            0f
        );

        // Spring physics
        Vector2 displacement = targetPos - pupil.anchoredPosition;
        Vector2 force = displacement * spring;
        velocity += force * Time.deltaTime;
        velocity *= (1f - damping * Time.deltaTime);

        // Apply movement
        pupil.anchoredPosition += velocity * Time.deltaTime;
    }
}
