using UnityEngine;

public class SetCullingMode : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void AlwaysAnimate()
    {
        if (animator != null)
        {
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }
    }

    public void CullUpdateTransforms()
    {
        if (animator != null)
        {
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        }
    }

    public void CullCompletely()
    {
        if (animator != null)
        {
            animator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }
}
