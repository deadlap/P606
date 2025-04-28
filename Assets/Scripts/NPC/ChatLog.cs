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
}
