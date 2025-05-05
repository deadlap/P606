using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float pitchVariation = 0.1f;
    [SerializeField, Range(0f, 1f)] private float volumeVariation = 0.1f;
    [SerializeField] private bool playOnAwake = false;
    private float startPitch = 1f;
    private float startVolume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        startPitch = audioSource.pitch;
        startVolume = audioSource.volume;

        audioSource.playOnAwake = false;
        audioSource.Stop();
    }

    private void OnEnable()
    {
        if (playOnAwake)
            PlaySound();
    }

    public void PlaySound(AudioClip clip = null)
    {
        audioSource.clip = clip == null ? audioSource.clip : clip;
        RandomisePitchVol();
        audioSource.Play();
    }

    private void RandomisePitchVol()
    {
        audioSource.pitch = startPitch + Random.Range(0f, pitchVariation) - 0.5f * pitchVariation;
        audioSource.volume = startVolume + Random.Range(0f, volumeVariation) - 0.5f * volumeVariation;
    }
}
