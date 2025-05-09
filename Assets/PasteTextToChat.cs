using TMPro;
using UnityEngine;

public class PasteTextToChat : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    
    public void PasteText()
    {
        if (!NewChatBot.instance.inputField.interactable || !NewChatBot.instance.isChatActive) return;
        NewChatBot.instance.GetText(textMeshProUGUI.text);
        NewChatBot.instance.AllowInput();
    }
}
