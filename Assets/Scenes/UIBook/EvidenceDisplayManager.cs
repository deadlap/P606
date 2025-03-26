using TMPro;
using UnityEngine;

public class EvidenceDisplayManager : MonoBehaviour
{
    public static EvidenceDisplayManager Instance;

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
}
