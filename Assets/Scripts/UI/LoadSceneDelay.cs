using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDelay : MonoBehaviour
{
    private float delay;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] Animator animator;
    
    void Start()
    {
        delay = audioClip.length;
    }

    public void PlaySoundThenLoad()
    {
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(FadeMusic(audioSource));
        StartCoroutine(FadeMusic(music));
        animator.Play("FadeOut");
        Invoke(nameof(LoadScene), delay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeMusic(AudioSource audio)
    {
        float startVolume = audio.volume;
        for (float t = 0; t < delay; t += Time.deltaTime)
        {
            audio.volume = Mathf.Lerp(startVolume, 0, t / delay);
            yield return null;
        }
        audio.volume = 0;
    }
}
