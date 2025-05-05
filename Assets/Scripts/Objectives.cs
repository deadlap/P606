using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
public class Objectives : MonoBehaviour {

    public enum ObjectiveEnum {
        CheckNoteBook,
        locateVictim,
        UncoverMurderer,
        AccuseMurderer,
        NewEvidence,
    }

    [SerializeField] TextMeshProUGUI TitleTextElement;
    [SerializeField] TextMeshProUGUI ObjectiveTextElement;
    [SerializeField] Animator Animator;
    [SerializeField] List<string> ObjectiveText;
    [SerializeField] List<string> ObjectiveTitleText;
    ObjectiveEnum currentObjective;

    public static event Action<ObjectiveEnum> ChangeTextEvent;
    public static void OnChangeTextEvent(ObjectiveEnum value) => ChangeTextEvent?.Invoke(value);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        currentObjective = 0;
        ChangeText();
    }
    void OnEnable() {
        ChangeTextEvent += UpdateText;
    }
    void OnDisable() {
        ChangeTextEvent -= UpdateText;
    }

    public void UpdateText(ObjectiveEnum objective) {
        if (objective == ObjectiveEnum.NewEvidence){
            StartCoroutine(DelayedReset(currentObjective, 4f));
        }
        Debug.Log("Objective changed to: " + objective.ToString());
        currentObjective = objective;
        Animator.Play("ChangeObjective");
        Invoke("ChangeText", 1f);
    }
    void ChangeText(){
        ObjectiveTextElement.text = ObjectiveText[(int)currentObjective];
        TitleTextElement.text = ObjectiveTitleText[(int)currentObjective];
    }
    private IEnumerator DelayedReset(ObjectiveEnum objective, float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateText(objective);
    }

}
