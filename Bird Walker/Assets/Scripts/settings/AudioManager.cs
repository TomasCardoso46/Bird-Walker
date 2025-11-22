using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private const string VolumeKey = "MasterVolume";
    private float masterVolume = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolume();
        ApplyVolume();
    }

    public void SetVolume(float value)
    {
        masterVolume = value;
        ApplyVolume();

        // Save
        PlayerPrefs.SetFloat(VolumeKey, masterVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        masterVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
    }

    private void ApplyVolume()
    {
        AudioListener.volume = masterVolume;
    }

    public float GetVolume()
    {
        return masterVolume;
    }
}
