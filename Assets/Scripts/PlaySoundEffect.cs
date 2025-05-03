using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip soundEffect;
    [SerializeField] [Range(0.5f, 1.5f)] float minPitch = 1, maxPitch = 1;
    [SerializeField] [Range(0.05f, 1f)] float startVolume = 1;
    void Awake()
    {
        if(audioSource != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1;
    }

    public void PlaySound()
    {
        if (audioSource != null && soundEffect != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = startVolume;
            audioSource.PlayOneShot(soundEffect);
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClip is not assigned to {name}.");
        }
    }
}
