using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Audio;

public class Ending : MonoBehaviour
{
    public static Ending instance { get; private set; }
    
    public GameObject blackScreen; // Reference to the black screen GameObject
    public GameObject Popup; // Reference to the popup GameObject aka, where the text saying, are you sure you wanna go further.

    public GameObject newspaper; // Reference to the newspaper GameObject

    public Animator WaxStampAnimator; // Reference to the animator for the wax stamp
    public GameObject WaxStamp; // Reference to the wax stamp GameObject
     public float fadeDuration = 2f; // Time it takes to fade in


    public BookManager bookManager; // Reference to the BookManager script
    public TextMeshProUGUI endingText; // Reference to the TextMeshProUGUI component for the ending text

    public Graphic blackScreenGraphic;
    public GameObject blackScreenImage; // Reference to the black screen image GameObject

    [Header ("Audio References")]
    [SerializeField] AudioSource audioSourceGoodEnding; // Reference to the AudioSource component
    [SerializeField] AudioSource audioSourceMediumEnding; // Reference to the AudioSource component for the neutral ending
    [SerializeField] AudioSource audioSourceBadEnding; // Reference to the AudioSource component for the bad ending
    [SerializeField] private AudioMixer gameplayAudio;

    public static event Action EndGameEvent;
    public static void OnEndGameEvent() => EndGameEvent?.Invoke();

    public void ShowPopup()
    {
        if (PersonIdentification.SomeoneIsSelected == true)
        {
            Popup.SetActive(true);
            bookManager.FreezeOrUnfreezeBook(true);
        }
        
       
    }
    void OnEnable() {
        EndGameEvent += BeginEnding;
    }
    void OnDisable() {
        EndGameEvent -= BeginEnding;
    }


    private void Awake()
    {
    // Ensure this is a singleton (only one of these should exist)
    if (instance != null)
    {
        Debug.Log($"There are multiple Ending Scripts, deleting {name}");
        Destroy(this);
        return;
    }

    instance = this;
    }

    public void HidePopup()
    {
        Popup.SetActive(false);
        bookManager.FreezeOrUnfreezeBook(false);
    }

    public void TriggerEnding() {
        OnEndGameEvent();
    }
    public void BeginEnding(){
        Popup.SetActive(false);

        WaxStamp.SetActive(true); // Show the wax stamp
        blackScreen.SetActive(true); // Show the black screen

        // Mute non-cutscene/ending stuff
        gameplayAudio.SetFloat("gameplayVol", Mathf.Log10(0.0001f) * 20f);

        // Stop player from moving
        PlayerController.instance.FreezePlayer(true);


        StartCoroutine(StartEnding());

    }

    public IEnumerator StartEnding()
    {
        {
        Color color = blackScreenGraphic.color;
        float startAlpha = color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 1f, time / fadeDuration);
            blackScreenGraphic.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure it ends exactly at full alpha
        blackScreenGraphic.color = new Color(color.r, color.g, color.b, 1f);
    }
        yield return new WaitForSeconds(1f); // 

        newspaper.SetActive(true); 
        if(GameTimer.INSTANCE.IsTimeUp() || bookManager.SelectedNPC.NPCIdentity.PrimaryRole != Identity.PrimaryRoles.Murderer) {
            endingText.text = "Passenger Wrongfully Accused of Murder by Rookie Detective: Murderer Still on the Loose!"; 
            audioSourceBadEnding.Play(); 
        } else {
            if (GameStats.INSTANCE.EvidenceGathered == GameStats.INSTANCE.EvidenceToGather) {
                endingText.text = "Brilliant Detective Strikes Again: Murder Case Solved with Mountains of Evidence!";
                audioSourceGoodEnding.Play();
            } else {
                endingText.text = "Prime Suspect Escapes Justice: Lack of Evidence Sinks Case in Court!";
                audioSourceMediumEnding.Play();
            }
        }

    }
}
