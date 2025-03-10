using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawnPointFinder : MonoBehaviour {

    public static NPCSpawnPointFinder Instance;
    [SerializeField] List<NPCSpawnPoint> UnusedSpawnPoints;
    [SerializeField] NPCSpawnPoint FallBackSpawnPoint;

    void Awake() {
        Instance = this;
        var temp_list = GameObject.FindGameObjectsWithTag("NPCSpawnPoint");
        foreach (GameObject obj in temp_list) {
            UnusedSpawnPoints.Add(obj.GetComponent<NPCSpawnPoint>());
        }
        UnusedSpawnPoints = ShuffleList(UnusedSpawnPoints);
    }

    public NPCSpawnPoint FindSpawnPoint(Identity.Occupations occupation){
        NPCSpawnPoint temp_spawn = null;
        temp_spawn = SearchListForSpawnTag(occupation);
        if (temp_spawn == null) {
            temp_spawn = SearchListForSpawnTag(Identity.Occupations.None);
        }
        if (temp_spawn == null && FallBackSpawnPoint != null) {
            return FallBackSpawnPoint;
        }
        return temp_spawn;
    }
    
    public NPCSpawnPoint SearchListForSpawnTag(Identity.Occupations occupation){
        for (int i = 0; i < UnusedSpawnPoints.Count; i++) {
            var temp_spawn = UnusedSpawnPoints[i];
            if (occupation == temp_spawn.OccupationTag){
                UnusedSpawnPoints.RemoveAt(i);
                return temp_spawn;
            }
        }
        return null;
    }
    public List<NPCSpawnPoint> ShuffleList(List<NPCSpawnPoint> list) {
		var count = list.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
        return list;
	}
}
