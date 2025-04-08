using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhoneticSoundPlayer : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    List<string> word;
    [SerializeField] [Range(-3, 3)] float pitchMin, pitchMax = 1; 
    [SerializeField] AudioClip[] phoneticSounds;
    Dictionary<char, AudioClip> phoneticSoundsMap = new();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        int index = 0;
        for (char c = 'a'; c <= 'z'; c++)
        {
            phoneticSoundsMap.Add(c, phoneticSounds[index]);
            index++;
        }
    }

    public void StartSpeak(string text)
    {
        StartCoroutine(SpeakPhonetic(text));
    }

    IEnumerator SpeakPhonetic(string text)
    {
        foreach (char letter in text.ToLower())
        {
            if (phoneticSoundsMap.ContainsKey(letter))
            {
                audioSource.pitch = Random.Range(pitchMin, pitchMax);
                audioSource.clip = phoneticSoundsMap[letter];
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);  
            }
            else
            {
                Debug.LogWarning("No sound assigned for character: " + letter);
            }
        }
    }
}
