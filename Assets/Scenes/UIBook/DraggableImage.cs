using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Net.WebSockets;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler

{
    [SerializeField] private bool endlessSpawner = false;

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

        if (currentSquare != null) currentSquare.AssignOccupant(this);
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
        // ----- FELIX KOMMENTAR -----
        // Tilføjede følgende 13 linjer til at erstatte hele draggable spawner scriptet, bør også betyde hele Update() kan fjernes
        if (endlessSpawner)
        {
            // Remember where in hiearchy this is
            int siblingIndex = transform.GetSiblingIndex();
            // Create a clone to stay here
            Transform myClone = Instantiate(gameObject, transform.parent).transform;
            // Make this no longer part of the grid
            transform.SetParent(transform.parent.parent);
            // Make sure the clone will be where this once was
            myClone.SetSiblingIndex(siblingIndex);
            // Make sure this no longer spawns clones
            endlessSpawner = false;
        }

        // Make this be rendered in front of others
        transform.SetAsLastSibling();

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

        // ❌ Snap failed or invalid → return to last position or destroy if no last position
        if (currentSquare == null)
        {
            Debug.Log($"{name} was dropped into nothing and had no place assigned as its home. It's gonna self destruct");
            Destroy(gameObject);
        }
        else
        {
            transform.position = currentSquare.transform.position;
        }
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

