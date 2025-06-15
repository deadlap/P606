using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class EvidenceSpawner : MonoBehaviour {
    [SerializeField] private GameObject evidencePrefab; // Prefab to spawn
    [SerializeField] private List<GameObject> spawnPoints; // Point where the evidence will be spawned
    public static List<int> NonDeathTimes;
    void Awake() {
        SpawnEvidence();
    }
    void Start() {
        NonDeathTimes = null;
        NonDeathTimes = new List<int>(GameStats.INSTANCE.Times);
        Debug.Log("Death time: " + (GameStats.INSTANCE.TimeOfDeath + GameStats.INSTANCE.ScheduleOffset));
        NonDeathTimes.Remove(GameStats.INSTANCE.TimeOfDeath + GameStats.INSTANCE.ScheduleOffset);
        Debug.Log(string.Join(", ", NonDeathTimes));
    }
    void SpawnEvidence() {
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("EvidenceSpawnPoint")); // Find all spawn points in the scene
        // Spawn evidence at random spawn points
        for (int i = 0; i < Enum.GetValues(typeof(Evidence.EvidenceType)).Length; i++) {
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            GameObject spawnPoint = spawnPoints[randomIndex];
            GameObject evidence = Instantiate(evidencePrefab, spawnPoint.transform);
            evidence.GetComponent<WorldEvidence>().evidenceType = (Evidence.EvidenceType)i;
            spawnPoints.RemoveAt(randomIndex); // Remove the spawn point to avoid spawning at the same location
        }
    }
}
