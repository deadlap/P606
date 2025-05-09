using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI UIText;
    [SerializeField] TextMeshPro worldText;
    [SerializeField] MeshRenderer planeBehindButton;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mouseOverAudio;
    [SerializeField] AudioClip mouseExitAudio;

    Color originalUITextColor;
    Material originalMaterial;
    Color originalWorldTextColor;
    [SerializeField] Color targetColor;
    [SerializeField] Material targetMaterial; 

    void Start()
    {
        if (UIText != null)
        {
            originalUITextColor = UIText.color;
        }
        if (worldText != null)
        {
            originalWorldTextColor = worldText.color;
        }
        if (planeBehindButton != null)
        {
            originalMaterial = planeBehindButton.material;
        }
        if(animator == null)
        {
            if(GetComponent<Animator>() != null)
            {
                animator = GetComponent<Animator>();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit();    
    }

    void MouseEnter()
    {
        if (audioSource != null && mouseOverAudio != null)
        {
            audioSource.PlayOneShot(mouseOverAudio);
        }
        if(animator != null)
        {
            animator.SetBool("MouseEnter", true);
            animator.SetBool("MouseExit", false);
        }
        if (UIText != null)
        {
            UIText.color = targetColor;
        }
        if (worldText != null)
        {
            worldText.color = targetColor;
        }
        if (planeBehindButton != null)
        {
            planeBehindButton.material = targetMaterial;
        }
    }

    void MouseExit()
    {
        if (audioSource != null && mouseExitAudio != null)
        {
            audioSource.PlayOneShot(mouseExitAudio);
        }
        if (animator != null)
        {
            animator.SetBool("MouseExit", true);
            animator.SetBool("MouseEnter", false);
        }
        if (UIText != null)
        {
            UIText.color = originalUITextColor;
        }
        if (worldText != null)
        {
            worldText.color = originalWorldTextColor;
        }
        if (planeBehindButton != null)
        {
            planeBehindButton.material = originalMaterial;
        }
    }
}
