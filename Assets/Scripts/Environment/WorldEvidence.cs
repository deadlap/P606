using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldEvidence : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Evidence.EvidenceType evidenceType; // Type of evidence
    [SerializeField] List<GameObject> typePrefabs; // Prefabs for each type of evidence
    bool interactedWith; // Flag to check if the player has interacted with the evidence
    void Start() {
        Instantiate(typePrefabs[(int)evidenceType], this.transform);
        interactedWith = false;
    }
    void OnEnable() {
        PlayerInputEvent.PlayerInteract += OnInteract; // Subscribe to the interact event
    }
    void OnDisable() {
        PlayerInputEvent.PlayerInteract -= OnInteract; 
    }
    public void OnInteract(){
        if (!GameStats.INSTANCE.IntroPlayed || interactedWith) return; // Check if the intro has been played and if the evidence has already been interacted with
        if (PlayerController.instance.currentInteractable != gameObject) return; // Check if the player is interacting with this object
        if (PlayerController.instance.currentInteractable == null) return; // Check if the player is interacting with anything
        interactedWith = true; // Set the interacted flag to true
        EvidenceDisplayManager.OnShowEvidenceEvent(evidenceType);
        Objectives.OnChangeTextEvent(Objectives.ObjectiveEnum.NewEvidence); // Update the objective text
        LogMaster.Instance.RememberEvidenceForLog(evidenceType); // Make LogMaster know of this evidence
        PlayerController.instance.audioSource.Play();
        Destroy(this.gameObject, 0.5f);
    }
}
