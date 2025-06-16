using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LoadSceneDelay : MonoBehaviour
{
    private float delay;
    [SerializeField] AudioMixer gameplayAudio;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] Animator animator;
    private bool buttonPressed;
    
    void Awake()
    {
        gameplayAudio.SetFloat("gameplayVol", Mathf.Log10(1f) * 20f);
        buttonPressed = false;
        delay = audioClip.length;
    }

    public void PlaySoundThenLoadTimeTrial(int sceneIndex)
    {
        GameTimer.IsTimeTrial = true;
        if (buttonPressed) return;
        buttonPressed = true;
        StartCoroutine(PlaySoundThenLoadScene(sceneIndex));
    }
    
    public void PlaySoundThenLoad(int sceneIndex)
    {
        GameTimer.IsTimeTrial = false;
        if (buttonPressed) return;
        buttonPressed = true;
        StartCoroutine(PlaySoundThenLoadScene(sceneIndex));
    }
    
    public void QuitGame()
    {
        if (buttonPressed) return;
        buttonPressed = true;
        Debug.LogError("GAME QUIT");
        Application.Quit();
    }

    IEnumerator PlaySoundThenLoadScene(int sceneIndex)
    {
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(FadeMusic(audioSource));
        StartCoroutine(FadeMusic(music));
        animator.Play("FadeOut");
        yield return new WaitForSeconds(delay);
        LoadScene(sceneIndex);   
    }
    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
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
