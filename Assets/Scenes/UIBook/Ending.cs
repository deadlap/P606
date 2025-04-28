using System.Collections;
using UnityEngine;
using TMPro;

public class Ending : MonoBehaviour
{
    public GameObject blackScreen;
    public GameObject Popup;

    public GameObject newspaper; // Reference to the newspaper GameObject
     public float fadeDuration = 2f; // Time it takes to fade in

    public TextMeshProUGUI endingText; // Reference to the TextMeshProUGUI component for the ending text

    private Renderer targetRenderer;
    private Color originalColor;


    public void Start()
    {
        targetRenderer = blackScreen.GetComponent<Renderer>();
        originalColor = targetRenderer.material.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        targetRenderer.material.color = transparentColor;
    }

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
        StartCoroutine(StartEnding());
    }

    public IEnumerator StartEnding()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            targetRenderer.material.color = newColor;
            yield return null;
        }

        // Ensure fully opaque at the end
        targetRenderer.material.color = originalColor;
        yield return new WaitForEndOfFrame(); 
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
