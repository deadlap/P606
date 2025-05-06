using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] Animator animator;
    void Awake()
    {
        animator.Play("FadeIn");
    }
}
