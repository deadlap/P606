using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject draggablePrefab; // This should be a prefab with DraggableImage on it
    [SerializeField] private Canvas canvas;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (draggablePrefab == null || canvas == null)
        {
            Debug.LogWarning("DraggableSpawner: Missing prefab or canvas reference.");
            return;
        }

        // Instantiate the draggable copy
        GameObject clone = Instantiate(draggablePrefab, canvas.transform);

        // Position it at the mouse pointer
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 worldPos
        );

        clone.GetComponent<RectTransform>().position = worldPos;

        // Begin dragging immediately
        var pointerDownHandler = clone.GetComponent<IPointerDownHandler>();
        pointerDownHandler?.OnPointerDown(eventData); // triggers drag
    }
}

