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

    public static event Action<Evidence.EvidenceType> ShowEvidenceEvent;
    public static void OnShowEvidenceEvent(Evidence.EvidenceType value) => ShowEvidenceEvent?.Invoke(value);

    void OnEnable() {
        ShowEvidenceEvent += ShowEvidence;
    }
    void OnDisable() {
        ShowEvidenceEvent -= ShowEvidence;
    }
    
    public void DisplayEvidence(Evidence evidence) {
        
        evidenceNameText.text = $"{evidence.evidenceName}";
        descriptionText.text = $"<b> DESCRIPTION </b>: \n \n{evidence.description}";
        infoText.text = $"<b> INFO </b>: \n \n{evidence.info}";

    }

    
   

    private void ShowEvidence(Evidence.EvidenceType evidenceType)
    {
        switch (evidenceType)
        {
            case Evidence.EvidenceType.Knife:
                knife.SetActive(true);
                knife.transform.SetAsLastSibling();
                break;
            case Evidence.EvidenceType.Pocket_Watch:
                pocketWatch.SetActive(true);
                pocketWatch.transform.SetAsLastSibling();
                break;
            case Evidence.EvidenceType.Diary:
                diary.SetActive(true);
                diary.transform.SetAsLastSibling();
                break;
            case Evidence.EvidenceType.Note:
                note.SetActive(true);
                note.transform.SetAsLastSibling();
                break;
        }
    }


}
