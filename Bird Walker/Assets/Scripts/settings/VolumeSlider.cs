using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;

    private void Start()
    {
        if (AudioManager.Instance != null)
            slider.value = AudioManager.Instance.GetVolume();

        // Update text on start
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetVolume(value);

        UpdateText(value);
    }

    private void UpdateText(float value)
    {
        // Example: "Volume: 75%"
        valueText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
