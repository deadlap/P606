using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public GameObject blackScreen; // Reference to the black screen GameObject
    public GameObject Popup; // Reference to the popup GameObject aka, where the text saying, are you sure you wanna go further.

    public GameObject newspaper; // Reference to the newspaper GameObject

    public Animator WaxStampAnimator; // Reference to the animator for the wax stamp
    public GameObject WaxStamp; // Reference to the wax stamp GameObject
     public float fadeDuration = 2f; // Time it takes to fade in

    public TextMeshProUGUI endingText; // Reference to the TextMeshProUGUI component for the ending text

    public Graphic blackScreenGraphic;
  
  
    public void ShowPopup()
    {
        Popup.SetActive(true);
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

        string endingType = gameObject.name; // Example: Use gameObject.name as the governing expression
        switch (endingType)
        {
            case "DidntCatch":
                endingText.text = "You didn't catch the killer. The case remains unsolved.";
                break;
            case "CatchNoEvidence":
                endingText.text = "You caught the killer, but there was no evidence to convict them.";
                break;
            case "CatchWithEvidence":
                endingText.text = "You caught the killer and gathered enough evidence to convict them.";
                break;
        }

    }
}
