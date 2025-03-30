using UnityEngine;
using UnityEngine.EventSystems;

public class PersonUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PersonNameDisplayManager displayManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (displayManager != null)
        {
            displayManager.DisplayName(gameObject.name);
        }
    }
}

