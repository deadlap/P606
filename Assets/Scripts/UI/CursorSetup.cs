using UnityEngine;

public class CursorSetup : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 cursorHotspot;
    [SerializeField] private CursorMode cursorMode;

    private void Awake()
    {
        SetupCursor();
    }

    private void SetupCursor()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, cursorMode);
    }
}
