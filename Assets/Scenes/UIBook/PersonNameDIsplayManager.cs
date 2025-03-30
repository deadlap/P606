using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PersonNameDisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainDisplayText; // The central display field for names
    [SerializeField] private CanvasGroup mainDisplayCanvasGroup; // Attach this to the same GameObject as mainDisplayText

    private Coroutine currentDisplayCoroutine;

    public void DisplayName(string name)
    {
        if (currentDisplayCoroutine != null)
            StopCoroutine(currentDisplayCoroutine);

        currentDisplayCoroutine = StartCoroutine(ShowNameRoutine(name));
    }

    private IEnumerator ShowNameRoutine(string name)
    {
        mainDisplayText.text = name;
        mainDisplayCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.5f); // Display for 0.5 seconds

        // Fade out over 0.5 seconds
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            mainDisplayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        mainDisplayText.text = "";
        currentDisplayCoroutine = null;
    }
}

