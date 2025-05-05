using UnityEngine;

public class DetectiveDoor : MonoBehaviour {
    bool HasTriggered;
    void Start() {
        HasTriggered = false;
    }
    void OnEnable() {
        Objectives.ChangeTextEvent += DisableDoor;
    }
    void OnDisable() {
        Objectives.ChangeTextEvent -= DisableDoor;
    }
    void DisableDoor(Objectives.ObjectiveEnum objective) {
        Debug.Log("DetectiveDoor: DisableDoor called with objective: " + objective);
        if (objective == Objectives.ObjectiveEnum.locateVictim && !HasTriggered) {
            Debug.Log("DetectiveDoor: Disabling door because objective is CheckNoteBook and HasTriggered is false.");
            HasTriggered = true;
            gameObject.SetActive(false);
        }
    }
}
