using UnityEngine;

public class NPCSpawnPoint : MonoBehaviour {
    public Identity.Occupations OccupationTag;
    public Vector3 SpawnPosition(){
        return transform.position;
    }    
}
