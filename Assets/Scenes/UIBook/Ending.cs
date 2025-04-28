using System.Collections;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject blackScreen;
    public GameObject Popup;

     public float fadeDuration = 2f; // Time it takes to fade in

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
    }
}
