using UnityEngine;
using UnityEngine.VFX;

public class DustCloud : MonoBehaviour
{
    [SerializeField] VisualEffect dustCloud;

    private void Awake()
    {
        dustCloud.Stop();
    }

    public void CreateDustCloud()
    {
        dustCloud.Play();
    }

    public void DisableDustCloud()
    {
        dustCloud.Stop();
    }
}
