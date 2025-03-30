using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSpawnerOnDrag : MonoBehaviour
{
    [SerializeField] private GameObject draggablePrefab;
    [SerializeField] private Canvas canvas;

    public void SpawnClone(PointerEventData eventData)
{
    if (draggablePrefab == null || canvas == null)
    {
        Debug.LogWarning("Missing prefab or canvas.");
        return;
    }

    // 🧠 Parent under the same page (not just the canvas)
    Transform parentPage = transform.parent;
    GameObject clone = Instantiate(draggablePrefab, parentPage);

    // Position it under the mouse
    RectTransformUtility.ScreenPointToWorldPointInRectangle(
        canvas.transform as RectTransform,
        eventData.position,
        eventData.pressEventCamera,
        out Vector3 worldPos
    );

    RectTransform cloneRect = clone.GetComponent<RectTransform>();
    cloneRect.position = worldPos;

    // Start manual drag
    var draggable = clone.GetComponent<DraggableImage>();
    if (draggable != null)
    {
        draggable.startDraggingNow = true;
    }

    Debug.Log("✅ Spawned under: " + parentPage.name);
}
}


