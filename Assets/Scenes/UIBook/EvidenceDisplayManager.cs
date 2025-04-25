using TMPro;
using UnityEngine;
using System;

public class EvidenceDisplayManager : MonoBehaviour
{
    public static EvidenceDisplayManager Instance;


    public enum EvidenceType{
        Knife,
        PocketWatch,
        Diary,
        Note,
    }
 
    public static event Action<EvidenceType> OnPickUpEvidence;

    [Header("Evidence")]

    public GameObject knife;
    public GameObject pocketWatch;
    public GameObject diary;
    public GameObject note;

   
   [Header("UI Elements")]

    public TextMeshProUGUI evidenceNumberText; 
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI ownerText;
    public TextMeshProUGUI mentionedByText;

    void Awake()
    {
        Instance = this;
    }


    
    public void DisplayEvidence(Evidence evidence)
    {
        
        evidenceNumberText.text = $"<b>Evidence {evidence.evidenceNumber}</b>";
        descriptionText.text = $"<b>Description:</b> {evidence.description}";
        ownerText.text = $"<b>Owner:</b> {evidence.owner}";
        mentionedByText.text = "<b>Mentioned By:</b>\n";

        foreach (string name in evidence.mentionedBy)
        {
            mentionedByText.text += "- " + name + "\n";
        }
    }

    
   

    private void ShowEvidence(EvidenceType evidenceType)
    {
        switch (evidenceType)
        {
            case EvidenceType.Knife:
                knife.SetActive(true);
                break;
            case EvidenceType.PocketWatch:
                pocketWatch.SetActive(true);
                break;
            case EvidenceType.Diary:
                diary.SetActive(true);
                break;
            case EvidenceType.Note:
                note.SetActive(true);
                break;
        }
    }


}
