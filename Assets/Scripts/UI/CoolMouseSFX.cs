using UnityEngine;
using UnityEngine.InputSystem;

public class CoolMouseSFX : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private UIAudioSource coolAudio;

    private void Awake()
    {
        coolAudio = GetComponent<UIAudioSource>();
    }

    void OnEnable()
    {
        if(playerInput == null) return;
        playerInput.actions["Click"].performed += DoMouseSound;
    }

    void OnDisable()
    {
        if (playerInput == null) return;
        playerInput.actions["Click"].performed -= DoMouseSound;
    }

    public void DoMouseSound(InputAction.CallbackContext ctx)
    {
        coolAudio.PlaySound();
    }
}
