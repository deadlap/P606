using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSpawnPointFinder : MonoBehaviour {

    public static NPCSpawnPointFinder Instance;
    // [SerializeField] Identity.Occupations Passengers;
    // [SerializeField] Identity.Occupations Workers;
    [SerializeField] List<NPCSpawnPoint> UnusedSpawnPoints;
    [SerializeField] NPCSpawnPoint FallBackSpawnPoint;

    void Awake() {
        // Passengers = Identity.Occupations.European_Painter 
        // | Identity.Occupations.Egg_Merchant | Identity.Occupations.Teacher | Identity.Occupations.Business_Person 
        // | Identity.Occupations.Mafioso | Identity.Occupations.Magician | Identity.Occupations.Entertainer | Identity.Occupations.Welder;

        // Workers = Identity.Occupations.Chef | Identity.Occupations.Janitor
        // | Identity.Occupations.Waiter | Identity.Occupations.Bartender;

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
        if (temp_spawn == null) {
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
