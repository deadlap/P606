using System.Collections;
using UnityEngine;
using Convai;
using Convai.Scripts.Runtime.Core;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign your NPC prefab in the Unity Inspector
    private NPC characterCreator;

    

    void Start()
    {
        characterCreator = gameObject.AddComponent<NPC>();
        StartCoroutine(characterCreator.CreateCharacter(SpawnNPC));
    }

    void SpawnNPC(string characterID)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)); // Random spawn position
        GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);

        ConvaiNPC convaiNPC = newNPC.GetComponent<ConvaiNPC>(); // Assuming Convai NPC script is attached
        if (convaiNPC != null)
        {
            convaiNPC.characterID = characterID;
            Debug.Log("Assigned Character ID to NPC: " + characterID);
        }
    }
}
