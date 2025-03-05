using UnityEngine;

public class NPCSpawnPointFinder : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] NPC Npc;
    [SerializeField] Identity NPCIdentity;
    
    void Start() {
        Npc = GetComponent<NPC>();
        NPCIdentity = GetComponent<Identity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
