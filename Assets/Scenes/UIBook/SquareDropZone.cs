using UnityEngine;
using System.Collections.Generic;

public class SquareDropZone : MonoBehaviour
{
    public static List<SquareDropZone> allDropZones = new List<SquareDropZone>();

    public float snapRadius = 50f;
    private RectTransform rectTransform;

    public DraggableImage currentOccupant;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (!allDropZones.Contains(this))
            allDropZones.Add(this);
    }

    private void OnDisable()
    {
        allDropZones.Remove(this);
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