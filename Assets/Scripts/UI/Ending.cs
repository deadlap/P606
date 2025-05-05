using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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

    public static event Action EndGameEvent;
    public static void OnEndGameEvent() => EndGameEvent?.Invoke();

    public void ShowPopup()
    {
        if (PersonIdentification.SomeoneIsSelected == true)
        {
            Popup.SetActive(true);
        }
        
       
    }
    void OnEnable() {
        EndGameEvent += TriggerEnding;
    }
    void OnDisable() {
        EndGameEvent -= TriggerEnding;
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
    }

    public void TriggerEnding()
    {
        Popup.SetActive(false);

        WaxStamp.SetActive(true); // Show the wax stamp
        blackScreen.SetActive(true); // Show the black screen
        StartCoroutine(StartEnding());
    }


    public void TriggerEndingTimeOut()
    {
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
            endingText.text = "You didn't catch the killer. The case remains unsolved."; 
            audioSourceBadEnding.Play(); 
        } else {
            if (GameStats.INSTANCE.EvidenceGathered == GameStats.INSTANCE.EvidenceToGather) {
                endingText.text = "You caught the killer, and with enough evidence, they were convicted of the murder";
                audioSourceGoodEnding.Play();
            } else {
                endingText.text = "You caught the killer, but you did not gather enough evidence for them to be convicted";
                audioSourceMediumEnding.Play();
            }
        }

    }
}
