using TMPro;
using UnityEngine;

public class NPCSpawnPoint : MonoBehaviour {
    public Identity.Occupations OccupationTag;
    
    public Vector3 SpawnPosition(){
        return transform.position;
    }
    private void Update()
    {
        if(OccupationTag.ToString() == "None")
            name = "Detective's Cabin";
        else
            name = $"{OccupationTag.ToString()}'s Cabin";
    }
}
