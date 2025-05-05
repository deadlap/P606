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
            OccupationTag.ToString().Replace("_", " ");
            name = $"{OccupationTag.ToString()}'s Cabin";
        }
    }
}
