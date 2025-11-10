using UnityEngine;

public class RandomPitchAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // Assign your AudioSource in the Inspector
    public AudioClip clip;          // Assign your audio clip in the Inspector

    [Header("Pitch Settings")]
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    [Header("Playback Settings")]
    public float startTime = 0f;    // Time in seconds to start the audio

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.P))
        {
            PlayRandomPitchAudioFromTime();
        }
    }

    void PlayRandomPitchAudioFromTime()
    {
        if (audioSource != null && clip != null)
        {
            // Assign clip
            audioSource.clip = clip;

            // Set a random pitch
            audioSource.pitch = Random.Range(minPitch, maxPitch);

            // Set the start time (make sure it's within clip length)
            audioSource.time = Mathf.Clamp(startTime, 0f, clip.length);

            // Play the audio
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or Clip is missing!");
        }
    }
}
