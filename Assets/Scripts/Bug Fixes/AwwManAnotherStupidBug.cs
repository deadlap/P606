using UnityEngine;

public class AwwManAnotherStupidBug : MonoBehaviour
{
    AudioSource audioSource;
    bool audioHasPlayedRecently = false;
    float time;
    float timeToWait = 10f; 
    bool hasPlayedFirstTime = false;

    void Awake()
    {
        hasPlayedFirstTime = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioHasPlayedRecently) return;
        time += Time.deltaTime;
        if (time >= timeToWait)
        {
            audioHasPlayedRecently = false;
            time = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource.isPlaying || audioHasPlayedRecently) return;
            audioSource.Play();
            audioHasPlayedRecently = true;
            if(!hasPlayedFirstTime)
            {
                BugfixManager.Instance.CountBugfixes();
                hasPlayedFirstTime = true;
            }
        }
    }
}
