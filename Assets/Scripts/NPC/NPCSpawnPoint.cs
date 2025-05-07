using TMPro;
using UnityEngine;

public class NPCSpawnPoint : MonoBehaviour {
    public Identity.Occupations OccupationTag;
    
    public Vector3 SpawnPosition(){
        return transform.position;
    }
    
    void Update()
    {
        if(OccupationTag.ToString() == "None")
            name = "Detective's Cabin";
        else
        {
            name = $"{OccupationTag.ToString().Replace("_", " ")}'s Cabin";
        }
    }
}
