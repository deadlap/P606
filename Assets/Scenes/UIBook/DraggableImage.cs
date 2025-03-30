using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler

{
    public bool startDraggingNow = false;
    private bool isDragging = false;


    private Vector3 originalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private SquareDropZone currentSquare = null; // ✅ Track where we're currently snapped
    private static List<SquareDropZone> dropZones = new List<SquareDropZone>();

    public float snapRadius = 50f;
    public float detachThreshold = 30f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalPosition = rectTransform.position;
    }

    private List<SquareDropZone> GetAvailableDropZones()
{
    return SquareDropZone.allDropZones;
}

    
    private void Start()
{
    

    // 🔍 Auto-snap check on start
    foreach (var square in dropZones)
    {
        float distance = Vector3.Distance(rectTransform.position, square.transform.position);
        if (distance < snapRadius)
        {
            if (!square.IsOccupied())
            {
                square.AssignOccupant(this);
                currentSquare = square;
                originalPosition = rectTransform.position;
                
            }
            break;
        }
    }
}


    private void Update()
{
    if (startDraggingNow)
    {
        startDraggingNow = false;

        // Simulate the pointer down logic
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); // bring to front
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

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            canvasGroup.blocksRaycasts = true;

            // Snap/drop logic
            SquareDropZone closestSquare = GetClosestDropZone();
            if (closestSquare != null && !closestSquare.IsOccupied())
            {
                rectTransform.position = closestSquare.transform.position;
                closestSquare.AssignOccupant(this);
                originalPosition = rectTransform.position;
            }
            else
            {
                rectTransform.position = originalPosition;
            }
        }
    }
}








    public void OnPointerDown(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetAsLastSibling();

        // ✅ Clear current square (we're moving away)
        if (currentSquare != null)
        {
            currentSquare.ClearOccupant();
            currentSquare = null;
        }
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
            float distance = Vector3.Distance(rectTransform.position, closestSquare.transform.position);

            if (distance <= snapRadius)
            {
                if (!closestSquare.IsOccupied())
                {
                    // ✅ Snap to square
                    rectTransform.position = closestSquare.transform.position;
                    closestSquare.AssignOccupant(this);
                    currentSquare = closestSquare;
                    originalPosition = rectTransform.position;
                    return;
                }
                
            }
        }

        // ❌ Snap failed or invalid → return to last position
        rectTransform.position = originalPosition;
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

