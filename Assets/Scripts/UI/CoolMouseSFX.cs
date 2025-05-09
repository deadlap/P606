using UnityEngine;
using UnityEngine.InputSystem;

public class CoolMouseSFX : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIAudioSource coolAudio;

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
