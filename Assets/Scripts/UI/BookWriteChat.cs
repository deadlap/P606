using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookWriteChat : MonoBehaviour
{
    private TextMeshProUGUI textField;
    private ContentSizeFitter contentSizeFitter;
    public static BookWriteChat instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        //textField = GetComponent<TextMeshProUGUI>();
    }

    //public void ReplaceText(ChatLog chatLog, string npcName)
    //{
    //    string coolFill = "Log:";
    //    for (int i = 0; i < chatLog.playerMessages.Count; i++)
    //    {
    //        coolFill += $"\nDetective:\n{chatLog.playerMessages[i]}\n{npcName}:\n{chatLog.npcMessages[i]}";
    //    }
    //    textField.text = coolFill;
    //}

    public void SpawnText(ChatLog chatLog, string npcName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < chatLog.playerMessages.Count; i++)
        {
            var playerMessage = ChatBubble.Instance.CreateChatBubble(chatLog.playerMessages[i], true, transform);
            var npcMessage = ChatBubble.Instance.CreateChatBubble(chatLog.npcMessages[i], false, transform);
        }
        contentSizeFitter.enabled = false;
        Invoke(nameof(EnableContentSizeFitter), 0.1f);
    }

    void EnableContentSizeFitter()
    {
        contentSizeFitter.enabled = true;
    }
}
