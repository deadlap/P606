using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldEvidence : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Evidence.EvidenceType evidenceType; // Type of evidence
    [SerializeField] List<GameObject> typePrefabs; // Prefabs for each type of evidence
    void Start() {
        Instantiate(typePrefabs[(int)evidenceType], this.transform);
    }
    void OnEnable() {
        PlayerInputEvent.PlayerInteract += OnInteract; // Subscribe to the interact event
    }
    void OnDisable() {
        PlayerInputEvent.PlayerInteract -= OnInteract; 
    }
    public void OnInteract(){
        if (!GameStats.INSTANCE.IntroPlayed) return; // Check if the intro has been played
        if (PlayerController.instance.currentInteractable != gameObject) return; // Check if the player is interacting with this object
        if (PlayerController.instance.currentInteractable == null) return; // Check if the player is interacting with anything
        EvidenceDisplayManager.OnShowEvidenceEvent(evidenceType);
        Objectives.OnChangeTextEvent(Objectives.ObjectiveEnum.NewEvidence); // Update the objective text
        Destroy(this.gameObject, 1f);
    }
}
