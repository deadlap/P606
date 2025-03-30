using UnityEngine;
using UnityEngine.EventSystems;

public class DummyDragable : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private DraggableSpawnerOnDrag spawner;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (spawner != null)
        {
            spawner.SpawnClone(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing — prevents dragging this object
    }
}

