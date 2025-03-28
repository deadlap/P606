using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomName : MonoBehaviour
{
    public static RoomName Instance;
    List<string> roomNames = new();
    TextMeshProUGUI roomText;
    string lastRoomName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        roomText = GetComponent<TextMeshProUGUI>();
    }

    public void AddRoomName(string roomName)
    {
        if(roomName == "") return;
        roomNames.Add(roomName);
        UpdateRoomName();
    }
    public void RemoveRoomName(string roomName)
    {
        if (roomName == "") return;
        lastRoomName = roomName;
        roomNames.Remove(roomName);
        UpdateRoomName();
    }

    void UpdateRoomName()
    {
        for (int i = 0; i < roomNames.Count; i++)
        {
            roomText.text = roomNames[i];
        }
        if (roomNames.Count == 0)
        {
            roomText.text = lastRoomName;
        }
    }
}
