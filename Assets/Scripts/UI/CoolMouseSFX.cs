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
        playerInput.actions["Click"].performed += DoMouseSound;
    }

    void OnDisable()
    {
        playerInput.actions["Click"].performed -= DoMouseSound;
    }

    public void DoMouseSound(InputAction.CallbackContext ctx)
    {
        coolAudio.PlaySound();
    }
}
