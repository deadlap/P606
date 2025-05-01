using System.Collections.Generic;
using UnityEngine;

public class WorldEvidence : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Evidence.EvidenceType evidenceType; // Type of evidence
    [SerializeField] List<GameObject> typePrefabs; // Prefabs for each type of evidence
    void Start() {
        Instantiate(typePrefabs[(int)evidenceType], this.transform);
    }
    void Update()
    {
        
    }
    public void OnInteract(){
        EvidenceDisplayManager.OnShowEvidenceEvent(evidenceType);
    }
}
