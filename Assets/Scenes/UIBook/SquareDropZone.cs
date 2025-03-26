using UnityEngine;

public class SquareDropZone : MonoBehaviour
{
    public float snapRadius = 50f;
    private RectTransform rectTransform;

    // 🔒 New: Track if someone is snapped here
    public DraggableImage currentOccupant;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Vector3 GetSnapPosition()
    {
        return rectTransform.position;
    }

    public bool IsOccupied()
    {
        return currentOccupant != null;
    }

    public void AssignOccupant(DraggableImage image)
    {
        currentOccupant = image;
    }

    public void ClearOccupant()
    {
        currentOccupant = null;
    }
}
