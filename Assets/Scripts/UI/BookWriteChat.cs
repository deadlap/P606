using UnityEngine;
using TMPro;

public class BookWriteChat : MonoBehaviour
{
    private TextMeshProUGUI textField;
    public static BookWriteChat instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        textField = GetComponent<TextMeshProUGUI>();
    }

    public void ReplaceText(ChatLog chatLog, string npcName)
    {
        string coolFill = "Log:";
        for (int i = 0; i < chatLog.playerMessages.Count; i++)
        {
            coolFill += $"\nDetective:\n{chatLog.playerMessages[i]}\n{npcName}:\n{chatLog.npcMessages[i]}";
        }
        textField.text = coolFill;
    }
}
