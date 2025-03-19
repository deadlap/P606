using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class NPC : MonoBehaviour
{
    private string convaiApiKey = "05da0da746e10cd320e9ddbf8c6e9e12"; // Replace with your actual Convai API Key
    private string characterName = "NPC_" + Random.Range(1000, 9999); // Random Name Generator

    public IEnumerator CreateCharacter(System.Action<string> callback)
    {
        string url = "https://api.convai.com/character/create";
        string jsonPayload = "{\"charName\":\"" + characterName + "\",\"voiceType\":\"MALE\",\"backstory\":\"A randomly generated NPC.\"}";
        Debug.Log("Sending API Key: " + convaiApiKey);
        Debug.Log("Full Request JSON: " + jsonPayload);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("CONVAI-API-KEY", convaiApiKey);
        request.SetRequestHeader("Accept", "application/json"); // <- ADD THIS LINE

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            JObject jsonResponse = JObject.Parse(request.downloadHandler.text);
            string generatedCharID = jsonResponse["charID"].ToString();
            Debug.Log("Generated Character ID: " + generatedCharID);
            callback(generatedCharID);
        }
        else
        {
            Debug.LogError("Error creating character: " + request.responseCode + " - " + request.error);
            Debug.LogError("Server Response: " + request.downloadHandler.text);
        }
    }
}

