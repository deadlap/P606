using System.Collections.Generic;
using UnityEngine;

public class Evidence : MonoBehaviour
{
    [Header("Evidence Info")]
    [TextArea] public string description;
    public string owner;
    public List<string> mentionedBy;

    public string evidenceNumber;


    public void OnClickEvidence()
    {
    ShowInfo();
    }   
    public void ShowInfo()
    {
        EvidenceDisplayManager.Instance.DisplayEvidence(this);
    }
}

