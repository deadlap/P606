using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
public class PersonIdentification : MonoBehaviour, IPointerClickHandler
{
    [Header("Person Info")]
    [SerializeField] private string personName;
    [SerializeField] private string occupation;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI occupationDisplay;
    [SerializeField] RawImage image;
    [SerializeField] RawImage rightSideImage;

    [Header("Double Click Settings")]

    
    //[SerializeField] private float doubleClickThreshold = 0.3f; // in seconds

    public static List<NPC> NPCS;
    NPC npc;

    private RenderTexture myPhoto;

    void Start() {

        if (NPCS == null){
            NPCS = new List<NPC>(NPCGenerator.INSTANCE.NPCs);
        }
        npc = NPCS[0];
        NPCS.RemoveAt(0);
        personName = npc.NPCIdentity.name;
        TakeNPCPicture photoCam = npc.GetComponent<TakeNPCPicture>();
        myPhoto = photoCam.outPutTexture;
        photoCam.TakePicture();
        image.texture = myPhoto;
        occupation = npc.NPCIdentity.Occupation.ToString().Replace("_", " ");
    }

    //private float lastClickTime = -1f;

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

        if (occupationDisplay != null)
            occupationDisplay.text = occupation;
        if (rightSideImage != null)
            rightSideImage.texture = myPhoto;
        
    }



}
