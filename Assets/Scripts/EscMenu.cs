using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    [SerializeField] GameObject fadedBackground;
    [SerializeField] Animator fadeAnimator;
    [SerializeField] AnimationClip returnToMainMenuEnd;
    [SerializeField] bool stopTime = false;
    bool isToggling;
    void Awake()
    {
        if (fadedBackground == null)
        {
            Debug.LogError("Faded background is not assigned in the inspector.");
            return;
        }
        fadedBackground.SetActive(false);
        PlayerInputEvent.escMenuOpen = false;
        PlayerInputEvent.isUIOpen = false;
    }
    void OnEnable()
    {
        PlayerInputEvent.CloseUI += EscPressed;
    }

    void OnDisable()
    {
        PlayerInputEvent.CloseUI -= EscPressed;
    }

    void EscPressed()
    {
        if(PlayerInputEvent.isUIOpen) return; // Prevents the menu from opening if another UI is open
        if (isToggling) return; // Prevents the menu from opening if it's already toggling
        if (!PlayerController.instance.canPlayerAct) return; // Prevents the menu from opening if the player is in a dialog or cutscene
        isToggling = true;
        if (!fadedBackground.activeSelf)
        {
            if(stopTime)
                Time.timeScale = 0f; // Pause the game
            Toggle();
            PlayerController.instance.Unsubscribe();
        }
        else
        {
            fadeAnimator.Play("ReturnToMainMenuEnd");
            if (stopTime)
                Time.timeScale = 1f; // Resume the game
            Invoke(nameof(Toggle), returnToMainMenuEnd.length);
            PlayerController.instance.Resubscribe();
        }
    }

    void Toggle()
    {
        isToggling = false;
        PlayerInputEvent.escMenuOpen = !PlayerInputEvent.escMenuOpen;
        fadedBackground.SetActive(!fadedBackground.activeSelf);
    }

    public void ClickYes()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ClickNo()
    {
        EscPressed();
        PlayerController.instance.Resubscribe();
    }
}
