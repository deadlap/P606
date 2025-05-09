using UnityEngine;

public class PreloadAClip : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        clip.LoadAudioData();
    }
}
