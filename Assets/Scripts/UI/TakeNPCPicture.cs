using UnityEngine;
using UnityEngine.UI;

public class TakeNPCPicture : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private RenderTexture basicTextureSetup;
    public RenderTexture outPutTexture { get; private set; }
    [SerializeField] private RawImage testShowcase;

    public void TakePicture()
    {
        testShowcase.texture = outPutTexture;
        m_Camera.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outPutTexture = new RenderTexture(basicTextureSetup);
        m_Camera.targetTexture = outPutTexture;
    }
}
