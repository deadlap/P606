using TMPro;
using UnityEngine;

public class PasteTextToChat : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    
    public void PasteText()
    {
        NewChatBot.instance.GetText(textMeshProUGUI.text);
    }
}
