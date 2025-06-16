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
    
    string previousText;
    
    public int lastSpokenIndex;
    bool newContent;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        int index = 0;
        for (char character = 'a'; character <= 'z'; character++)
        {
            phoneticSoundsMap.Add(character, phoneticSounds[index]);
            index++;
        }
    }

    public void StartSpeak(string text)
    {
        StartCoroutine(SpeakPhonetic(GetNewText(text)));
    }

    string GetNewText(string text)
    {
        if (text.Length <= lastSpokenIndex)
        {
            // No new content to speak
            newContent = false;
            return "";
        }
        newContent = true;

        string newPart = text.Substring(lastSpokenIndex);
        lastSpokenIndex = text.Length;

        if (!string.IsNullOrEmpty(newPart))
        {
            print($"\"{newPart}\" will now be spoken by the NPC");
        }

        return newPart;
    }
    //string GetNewText(string text)
    //{
    //    if (string.IsNullOrEmpty(previousText))
    //    {
    //        previousText = text;
    //        print($"\"{text}\" will now be spoken by the NPC");
    //        return text;
    //    }

    //    int minLength = Mathf.Min(previousText.Length, text.Length);
    //    int commonPrefixLength = 0;

    //    for (int i = 0; i < minLength; i++)
    //    {
    //        if (previousText[i] != text[i])
    //            break;
    //        commonPrefixLength++;
    //    }

    //    string newPart = text.Substring(commonPrefixLength);

    //    previousText = text;

    //    if (!string.IsNullOrEmpty(newPart))
    //    {
    //        print($"\"{newPart}\" will now be spoken by the NPC");
    //    }

    //    return newPart;
    //}

    IEnumerator SpeakPhonetic(string word)
    {
        if(newContent) yield return null;
        word = word.ToLower();
        for (int i = 0; i < word.Length; i++)
        {
            if (phoneticSoundsMap.ContainsKey(word[i]))
            {
                // Would make the pitch higher if next character is a questionmark (which it will never be due to the way text Streaming of the LLM Character is)
                if (i < word.Length)
                {
                    if (word[i+1] == '?')
                    {
                        Debug.Log("Question mark detected, increasing pitch.");
                        audioSource.pitch += 0.2f;
                    }
                    else
                    {
                        audioSource.pitch = Random.Range(pitchMin, pitchMax);
                    }
                }
                else
                {
                    audioSource.pitch = Random.Range(pitchMin, pitchMax);
                }
                audioSource.clip = phoneticSoundsMap[word[i]];
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);  
            }
            else
            {
                Debug.LogWarning("No sound assigned for character: " + word[i]);
            }
        }
    }
}
