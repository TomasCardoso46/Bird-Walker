using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimingSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;

    private void Start()
    {
        if (TimingManager.Instance != null)
            slider.value = TimingManager.Instance.GetTiming();

        // Update text on start
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (TimingManager.Instance != null)
            TimingManager.Instance.SetTiming(value);

        UpdateText(value);
    }

    private void UpdateText(float value)
    {
        // Example: "Timing: 1.5"
        valueText.text = value.ToString("0.00");
    }
}
