using UnityEngine;
using Piper;
using System.Collections.Generic;

public class PiperTTS : MonoBehaviour
{
    [SerializeField] PiperManager piperManager;
    AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //this method is async because it must await the audio generation.
    public async void OnInputSubmit(string text)
    {
        //Starts a stopwatch to see how fast it generates the speech.
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        
        var audio = piperManager.TextToSpeech(text); 
        print($"PiperTTS Generation time: {sw.ElapsedMilliseconds} ms");
        
        //stop the audio source and destroy the clip if it is playing.
        audioSource.Stop();
        if (audioSource && audioSource.clip)
            Destroy(audioSource.clip);

        //since it takes a little time to generate the audio, an await operator is used.
        audioSource.clip = await audio;
        audioSource.Play();
    }
}
