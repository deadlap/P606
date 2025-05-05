using System.Collections.Generic;
using UnityEngine;

public class ChatLog : MonoBehaviour
{
    public List<string> playerMessages = new();
    public List<string> npcMessages = new();

    public void AddTextToLog(string playerMessage, string npcMessage)
    {
        playerMessages.Add(playerMessage);
        npcMessages.Add(npcMessage);
    }

    #region For end logging
    public static List<ChatLog> chatLogs { get; private set; } = new();

    private void OnEnable()
    {
        chatLogs.Add(this);
    }

    private void OnDisable()
    {
        chatLogs.Remove(this);
    }

    public void DoLogging()
    {
        LogMaster.Instance.AddLine($"{name} prompts sent:{playerMessages.Count}");
    }
    #endregion
}
