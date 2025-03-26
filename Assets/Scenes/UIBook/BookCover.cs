using UnityEngine;
using UnityEngine.UI;

public class BookCover : MonoBehaviour
{
    [SerializeField] private RawImage previewImage;

    public void ShowPreview(Texture texture)
    {
        if (previewImage != null && texture != null)
        {
            previewImage.texture = texture;
            previewImage.enabled = true; // in case it's hidden
        }
    }

    public void ClearPreview()
    {
        if (previewImage != null)
        {
            previewImage.texture = null;
            previewImage.enabled = false;
        }
    }
}
