using TMPro;
using UnityEngine;
using System;

public class EvidenceDisplayManager : MonoBehaviour
{
    public static EvidenceDisplayManager Instance;
 
    public static event Action<Evidence.EvidenceType> OnPickUpEvidence;

    [Header("Evidence")]

    public GameObject knife;
    public GameObject pocketWatch;
    public GameObject diary;
    public GameObject note;

   
   [Header("UI Elements")]

    public TextMeshProUGUI evidenceNameText; 
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI infoText;

    void Awake()
    {
        Instance = this;
    }


    
    public void DisplayEvidence(Evidence evidence)
    {
        
        evidenceNameText.text = $"<b>{evidence.evidenceName}</b>";
        descriptionText.text = $"<b>Description:</b> {evidence.description}";
        infoText.text = $"<b>Info:</b> {evidence.info}";

    }

    
   

    private void ShowEvidence(Evidence.EvidenceType evidenceType)
    {
        switch (evidenceType)
        {
            case Evidence.EvidenceType.Knife:
                knife.SetActive(true);
                break;
            case Evidence.EvidenceType.Pocket_Watch:
                pocketWatch.SetActive(true);
                break;
            case Evidence.EvidenceType.Diary:
                diary.SetActive(true);
                break;
            case Evidence.EvidenceType.Note:
                note.SetActive(true);
                break;
        }
    }


}
