using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PersonIdentification : MonoBehaviour, IPointerClickHandler
{
    [Header("Person Info")]
    [SerializeField] private string personName;
    [SerializeField] private int personAge;
    [SerializeField] private string occupation;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI ageDisplay;
    [SerializeField] private TextMeshProUGUI occupationDisplay;

    [Header("Double Click Settings")]

    
    [SerializeField] private float doubleClickThreshold = 0.3f; // in seconds

    

    private float lastClickTime = -1f;

    public static bool SomeoneIsSelected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        
        ShowPersonInfo();
        /*float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            ShowPersonInfo();
        }

        lastClickTime = Time.time;
        */
    }

    private void ShowPersonInfo()
    {
        SomeoneIsSelected = true;

        if (nameDisplay != null)
            nameDisplay.text = personName;

        if (ageDisplay != null)
            ageDisplay.text = "" + personAge;

        if (occupationDisplay != null)
            occupationDisplay.text = occupation;
    }
}
