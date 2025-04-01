using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler

{
    public bool startDraggingNow = false;
    private bool isDragging = false;


    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    [SerializeField] private SquareDropZone currentSquare = null; // ✅ Track where we're currently snapped

    public float snapRadius = 50f;
    public float detachThreshold = 30f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (currentSquare == null) Debug.LogWarning($"{name} is not assigned a square in the book and therefore will not function correctly");
        currentSquare.AssignOccupant(this);
    }


    private void Update()
{
    if (startDraggingNow)
    {
        startDraggingNow = false;

        // Simulate the pointer down logic
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
    }

    // If we are manually dragging, follow the mouse
    if (isDragging)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }
}








    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        SquareDropZone closestSquare = GetClosestDropZone();

        if (closestSquare != null)
        {
            if (!closestSquare.IsOccupied())
            {
                // ✅ Clear current square (we're moving away)
                if (currentSquare != null)
                {
                    currentSquare.ClearOccupant();
                    currentSquare = null;
                }

                // ✅ Snap to square
                transform.position = closestSquare.transform.position;
                closestSquare.AssignOccupant(this);
                currentSquare = closestSquare;
                return;
            }
        }

        // ❌ Snap failed or invalid → return to last position
        transform.position = currentSquare.transform.position;
    }

    private SquareDropZone GetClosestDropZone()
{
    SquareDropZone closest = null;
    float minDistance = float.MaxValue;

    foreach (var dropZone in SquareDropZone.allDropZones)
    {
        if (dropZone == null || !dropZone.gameObject.activeInHierarchy)
            continue;

        float distance = Vector3.Distance(rectTransform.position, dropZone.transform.position);
        if (distance < snapRadius && distance < minDistance)
        {
            closest = dropZone;
            minDistance = distance;
        }
    }

    return closest;
}

    public void OnPointerClick(PointerEventData eventData)
{
    if (eventData.clickCount == 1) // optional: you could make this double click if you want
    {
        BookCover previewer = FindFirstObjectByType<BookCover>();
        if (previewer != null)
        {
            RawImage sourceImage = GetComponent<RawImage>();
            if (sourceImage != null)
            {
                previewer.ShowPreview(sourceImage.texture);
            }
        }
    }
}


}

